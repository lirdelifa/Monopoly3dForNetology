using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using Zenject;

public class WorldSpaceSystem : IDispose
{
    [Inject] private Player _player;
    [Inject] private TopCharacterInformation _topCharacterInformation;
    private Cell _currentMarkedCell;

    WorldSpaceSystem()
    {
        Character.OnEndFlip += EndFlip;
        Character.OnEndMove += EndMove;
        Intellect.OnEndTurn += EndTurn;
        Player.OnPlayerStartTurn += PlayerStartTurn;
        Player.OnPlayerEndTurn += PlayerEndTurn;
        Player.OnChanges += PlayerChanges;
    }

    private void PlayerStartTurn()
    {
        _topCharacterInformation.Show(_player.CurrentCharacter);
    }

    private void PlayerEndTurn() 
    {
        _topCharacterInformation.Hide();
    }

    private void PlayerChanges()
    {

        if(_player.CurrentCharacter != null && _player.CurrentCharacter.IsMyTurn) _topCharacterInformation.Show(_player.CurrentCharacter);
    }


    private void EndFlip(Character character)
    {
        if (!character.IsMyTurn) return;
        _currentMarkedCell = CellsData.GetNextCell(CellsData.DefiningCellIdByPosition(character.GetPoint), character.FellOutNumber);
        EnableMarkerArrow(true);
    }

    private void EndMove(Character character)
    {
        if (!character.IsMyTurn) return;
        EnableMarkerArrow(false);
    }

    private void EndTurn(bool changeId)
    {
        EnableMarkerArrow(false);
    }

    private void EnableMarkerArrow(bool isEnable)
    {
        if (_currentMarkedCell == null) return;
        _currentMarkedCell.EnableMarkerArrow(isEnable);
    }

    public void Dispose()
    {
        Character.OnEndFlip -= EndFlip;
        Character.OnEndMove -= EndMove;
        Intellect.OnEndTurn -= EndTurn;
        Player.OnPlayerStartTurn -= PlayerStartTurn;
        Player.OnPlayerEndTurn -= PlayerEndTurn;
        Player.OnChanges -= PlayerChanges;
    }
}
