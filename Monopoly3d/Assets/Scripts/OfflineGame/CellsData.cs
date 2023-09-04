using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsData : MonoBehaviour
{
    [SerializeField] public List<Cell> _cells;
    private readonly List<int> _cornerCellsId = new List<int>() { 0, 9, 18, 27 };

    [SerializeField] private List<CellGroupParameters> _cellsGroups;

    private void Start()
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            _cells[i].SetId(i);
            if(_cells[i] is StandartCell)
            {
                StandartCell standartCell = (StandartCell)_cells[i];
                for (int b = 0; b < _cellsGroups.Count; b++)
                {
                    if (_cellsGroups[b].type == standartCell.GetGroupType)
                    {
                        _cellsGroups[b].cells.Add(standartCell);
                        standartCell.SetMaterial(_cellsGroups[b].material);
                        standartCell.SetGroupName(_cellsGroups[b].name);
                    }
                }
            }
        }
    }

    public Cell GetStartCell { get => _cells[0]; }
    public int GetStartCellId { get => 0; }
    
    public Cell GetCornerCellOnRay(int from, int to)
    {

        int newTo = (from > to) ? to + _cells.Count : to;



        for(int i = from + 1; i < newTo; i++) 
        {
            for(int b = 0; b < _cornerCellsId.Count; b++)
            {
                if (i == _cornerCellsId[b]) return _cells[i];
                if (i >= _cells.Count && (i - _cells.Count) == _cornerCellsId[b]) return _cells[i - _cells.Count];
            }
        }
        return _cells[to];
    }

    public int GetCellIdByCell(Cell cell)
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            if (_cells[i] == cell) return i;
        }
        return 0;
    }

    public int GetCellIdByPosition(Vector3 position)
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            if (_cells[i].transform.position == position) return i;
        }
        return 0;
    }
}
