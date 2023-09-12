using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ChangeButtonActionTypesSystem : IDispose
{
    [Inject] private Player _player;

    private List<IRaycasterListener> _raycasterListeners;

    public void AddAllRaycasterListener(List<IRaycasterListener> raycasterListeners)
    {
        _raycasterListeners = raycasterListeners;
        ChangeButtonActionTypes();
    }

    ChangeButtonActionTypesSystem() 
    {
        Player.OnChanges += ChangeButtonActionTypes;
    }

    private void ChangeButtonActionTypes()
    {
        for(int i = 0; i < _raycasterListeners.Count; i++)
        {
            List<ButtonActionType> buttonActionTypes = new List<ButtonActionType>();

            //Юниты
            if (_raycasterListeners[i] is Character)
            {
                buttonActionTypes.Add(ButtonActionType.Select);
                buttonActionTypes.Add(ButtonActionType.Info);
                if (_player.GetControlledCharacters.Contains((Character)_raycasterListeners[i]))
                {
                    buttonActionTypes.Add(ButtonActionType.RemoveCharacter);
                }
                else
                {
                    buttonActionTypes.Add(ButtonActionType.AddCharacter);
                    if (_player.CurrentCharacter != null && _player.CurrentCharacter.CanShoot)
                    {
                        buttonActionTypes.Add(ButtonActionType.Shoot);
                    }
                }
            }
            //Таргеты
            else if (_raycasterListeners[i] is TargetPoint)
            {
                TargetPoint targetPoint = (TargetPoint)_raycasterListeners[i];
                if (targetPoint.IsFree && _player.CurrentCharacter != null)
                {
                    if(_player.CurrentCharacter.CanMove)
                    {
                        if (CellsData.GetCellById(CellsData.DefiningCellIdByPosition(targetPoint.GetPoint)) == CellsData.GetNextCell(CellsData.DefiningCellIdByPosition(_player.CurrentCharacter.GetPoint), _player.CurrentCharacter.FellOutNumber))
                        {
                            buttonActionTypes.Add(ButtonActionType.Move);
                        }
                    }
                }
                if(CellsData.GetCellById(CellsData.DefiningCellIdByPosition(targetPoint.GetPoint)) is CellProperty)
                {
                    buttonActionTypes.Add(ButtonActionType.Info);
                    CellProperty cellProperty = (CellProperty)CellsData.GetCellById(CellsData.DefiningCellIdByPosition(targetPoint.GetPoint));
                    if(_player.CurrentCharacter != null)
                    {
                        if(CellsData.GetCellById(CellsData.DefiningCellIdByPosition(_player.CurrentCharacter.GetPoint)) == cellProperty)
                        {
                            if(cellProperty.Owner == null) buttonActionTypes.Add(ButtonActionType.Buy);
                        }
                        if (cellProperty.Owner == _player.CurrentCharacter) buttonActionTypes.Add(ButtonActionType.Sell);

                    }
                }
            }

            //Кубики
            else if (_raycasterListeners[i] is Dice)
            {
                buttonActionTypes.Add(ButtonActionType.Select);
                if (_player.CurrentCharacter != null && _player.CurrentCharacter.CanFlip)
                {
                    buttonActionTypes.Add(ButtonActionType.Flip);
                }
            }

            //Стены
            else if (_raycasterListeners[i] is ProtectionWall)
            {
                ProtectionWall protectionWall = (ProtectionWall)_raycasterListeners[i];
                buttonActionTypes.Add(ButtonActionType.Select);
                if (_player.CurrentCharacter != null)
                {
                    if (protectionWall.GetCell.Owner == _player.CurrentCharacter)
                    {
                        if(protectionWall.IsShow) buttonActionTypes.Add(ButtonActionType.Sell);
                        else buttonActionTypes.Add(ButtonActionType.Buy);
                    }
                }
            }


            _raycasterListeners[i].ChangeButtonActionTypes(buttonActionTypes);
        }
    }

    public void Dispose()
    {
        Player.OnChanges -= ChangeButtonActionTypes;
    }
}
