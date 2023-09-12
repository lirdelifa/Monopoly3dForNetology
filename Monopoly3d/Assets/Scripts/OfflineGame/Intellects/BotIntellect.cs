using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BotIntellect : Intellect
{
    private const int _probabilityOfBuyingCell = 100; //70
    private const int _probabilityOfUpgradingCell = 100; // 50


    public override void AddCharacter(Character character)
    {
        base.AddCharacter(character);
        character.ChangeControlledIntelect(false);
        if (character.IsMyTurn) DefiningAction(character);
    }

    private void DefiningAction(Character character)
    {
        if (!character.IsMyTurn) return;
        if (character.CanEndTurn)
        {
            EndTurn(character);
        }
        else if (character.CanFlip)
        { 
            character.Flip();
        }
        else if (character.CanMove)
        {
            Vector3 targetPoint = CellsData.GetFreePointByCell(CellsData.GetNextCell(CellsData.DefiningCellIdByPosition(character.GetPoint), character.FellOutNumber));
            List<Vector3> points = CellsData.GeneratePathToPoint(character.GetPoint, targetPoint);
            character.Move(points);
        }
        else if (character.CanShoot)
        {
            Vector3 targetPoint = CharactersData.GetCharactersExcept(character)[Random.Range(0, CharactersData.GetCountCharacters - 1)].GetPoint;
            character.Shoot(targetPoint);
        }
    }

    protected override void SellOnNeedMoney(Character character, int needMoney)
    {
        _moneyCountingSystem.SellOnNeedMoney(character, needMoney);
    }

    private void TryBuy(Character character)
    {
        if (Random.Range(1, 101) < _probabilityOfBuyingCell)
        {
            _moneyCountingSystem.TryBuyCell(character);
        }
        if (Random.Range(1, 101) < _probabilityOfUpgradingCell)
        {
            _moneyCountingSystem.TryBuyRandUpgrade(character);
        }
    }

    protected override void OnCharacterAction(CharacterActionType actionType, Character character)
    {
        if (_isGameOver) return;
        if (actionType == CharacterActionType.EndMove) TryBuy(character);
        base.OnCharacterAction(actionType, character);
        DefiningAction(character);
    }
}
