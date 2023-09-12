using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;

public class StoppingAtCellSystem : IDispose
{
    [Inject] private MoneyCountingSystem _moneyCountingSystem;
    [Inject] private CardPanel _cardPanel;

    private int _lastCellId;
    StoppingAtCellSystem()
    {
        Character.OnStartMove += SetLastCellId;
        Character.OnEndMove += StoppingAtCell;
    }

    public void Dispose()
    {
        Character.OnStartMove -= SetLastCellId;
        Character.OnEndMove -= StoppingAtCell;
    }

    private void SetLastCellId(Character character)
    {
        if (!character.IsMyTurn) return;
        _lastCellId = CellsData.DefiningCellIdByPosition(character.GetPoint);
    }
   

    private void StoppingAtCell(Character character)
    {
        if (!character.IsMyTurn) return;

        int cellId = CellsData.DefiningCellIdByPosition(character.GetPoint);

        if (cellId < _lastCellId)
        {
            Debug.Log("Добавлены деньги за стартовую ячейку");
            _moneyCountingSystem.AddMoneyForStartCell(character);
        }

        CellProperty cellProperty = CellsData.GetCellPropertyById(cellId);
        if (cellProperty != null) 
        {
            if (cellProperty.Owner != null && cellProperty.Owner != character)
            {
                character.TryPay();
                return;
            }
        }
        if(CellsData.TeleportCellsId.Contains(cellId))
        {
            int nextTeleportCellId = 0;
            for(int i = 0; i < CellsData.TeleportCellsId.Count; i++)
            {
                if (CellsData.TeleportCellsId[i] == cellId)
                {
                    if (i == CellsData.TeleportCellsId.Count - 1) nextTeleportCellId = 0;
                    else nextTeleportCellId = i + 1;
                    break;
                }
            }
            Vector3 point = CellsData.GetFreePointByCell(CellsData.GetCellById(CellsData.TeleportCellsId[nextTeleportCellId]));
            Vector3 newPosition = new Vector3(point.x, character.GetPoint.y, point.z);
            character.transform.position = newPosition;
        }
        if (CellsData.DropCellsId.Contains(cellId))
        {
            _cardPanel.ShowRandomDropCard(character);
            return;
        }
        if (CellsData.ChanceCellsId.Contains(cellId))
        {
            _cardPanel.ShowRandomChanceCard(character);
            return;
        }
        if (CellsData.ArrestCellId == cellId) 
        {
            Vector3 point = CellsData.GetFreePointByCell(CellsData.GetPrisonCell);
            Vector3 newPosition = new Vector3(point.x, character.GetPoint.y, point.z);
            character.transform.position = newPosition;
            character.Arrest();
            return;
        }
        character.Resume();
    }
}
