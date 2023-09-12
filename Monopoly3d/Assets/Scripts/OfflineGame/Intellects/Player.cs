using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Zenject;

public class Player : Intellect 
{
    [Inject] private BotIntellect _botIntellect;
    public Character CurrentCharacter { get; private set; }
    public List<Character> GetControlledCharacters { get => _controlledCharacters; }

    Player()
    {
        ActionCircle.OnMoveToPoint += MoveToPoint;
        ActionCircle.OnTransferCharacter += TransferCharacter;
        ActionCircle.OnFlip += Flip;
        ActionCircle.OnShoot += Shoot;
        ActionCircle.OnBuy += Buy;
        ActionCircle.OnSell += Sell;
        TopCharacterInformation.OnEndTurn += DownEndTurnButton;
    }

    private void Buy(IRaycasterListener raycasterListener)
    {
        if (CurrentCharacter == null) return;
        if(raycasterListener is TargetPoint)
        {
            _moneyCountingSystem.TryBuyCell(CurrentCharacter);
        }
        else if (raycasterListener is ProtectionWall)
        {
            _moneyCountingSystem.TryBuyUpgrade(CurrentCharacter, (ProtectionWall)raycasterListener);
        }
        OnChanges?.Invoke();
    }

    private void Sell(IRaycasterListener raycasterListener)
    {
        if (CurrentCharacter == null) return;
        if (raycasterListener is TargetPoint)
        {
            _moneyCountingSystem.SellCell(CurrentCharacter, CellsData.GetCellPropertyById(CellsData.DefiningCellIdByPosition(raycasterListener.GetPoint)));
        }
        else if (raycasterListener is ProtectionWall)
        {
            _moneyCountingSystem.SellUpgrade(CurrentCharacter, (ProtectionWall)raycasterListener);
        }
        OnChanges?.Invoke();
    }

    private void DownEndTurnButton()
    {
        EndTurn(CurrentCharacter);
    }

    private void Shoot(Vector3 point)
    {
        CurrentCharacter.Shoot(point);
        OnChanges?.Invoke();
    }

    private void Flip()
    {
        CurrentCharacter.Flip();
        OnChanges?.Invoke();
    }

    private void MoveToPoint(Vector3 point)
    {
        List<Vector3> points = CellsData.GeneratePathToPoint(CurrentCharacter.GetPoint, point);
        CurrentCharacter.Move(points);
        OnChanges?.Invoke();
    }

    private void TransferCharacter(IRaycasterListener character, bool adding)
    {
        if (adding) AddCharacter((Character)character);
        else RemoveCharacter((Character)character);
        OnChanges?.Invoke();
    }

    public override void AddCharacter(Character character)
    {
        base.AddCharacter(character);
        character.ChangeControlledIntelect(true);
        _botIntellect.RemoveCharacter(character);
        if(character.IsMyTurn)
        {
            CurrentCharacter = character;
            OnPlayerStartTurn?.Invoke();
        }
    }
    public override void RemoveCharacter(Character character)
    {
        base.RemoveCharacter(character);
        _botIntellect.AddCharacter(character);
        if(character == CurrentCharacter)
        {
            OnPlayerEndTurn?.Invoke();
            CurrentCharacter = null;
        }
    }


    protected override void SellOnNeedMoney(Character character, int needMoney)
    {
        _moneyCountingSystem.SellOnNeedMoney(character, needMoney);
    }

    protected override void OnCharacterAction(CharacterActionType actionType, Character character)
    {
        if (_isGameOver) return;
        base.OnCharacterAction(actionType, character);
     
        if (actionType == CharacterActionType.StartTurn)
        {
            CurrentCharacter = character;
            OnPlayerStartTurn?.Invoke();
        }

        OnChanges?.Invoke();
    }

    protected override void EndTurn(Character character)
    {
        OnPlayerEndTurn?.Invoke();
        CurrentCharacter = null;
        base.EndTurn(character);
    }

    public override void Dispose()
    {
        base.Dispose();
        ActionCircle.OnMoveToPoint -= MoveToPoint;
        ActionCircle.OnTransferCharacter -= TransferCharacter;
        ActionCircle.OnFlip -= Flip;
        ActionCircle.OnShoot -= Shoot;
        ActionCircle.OnBuy -= Buy;
        ActionCircle.OnSell -= Sell;
        TopCharacterInformation.OnEndTurn -= DownEndTurnButton;
    }

    public delegate void ChangesDelegate();
    public static event ChangesDelegate OnChanges;

    public delegate void PlayerStartTurnDelegate();
    public static event PlayerStartTurnDelegate OnPlayerStartTurn;

    public delegate void PlayerEndTurnDelegate();
    public static event PlayerEndTurnDelegate OnPlayerEndTurn;
}
