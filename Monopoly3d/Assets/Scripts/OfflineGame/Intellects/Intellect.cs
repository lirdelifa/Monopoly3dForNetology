using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Zenject;

public class Intellect : IDispose
{
    [Inject] protected MoneyCountingSystem _moneyCountingSystem;
    [Inject] private GameOverPanel _gameOverPanel;
    protected List<Character> _controlledCharacters = new List<Character>();
    protected bool _isGameOver;

    protected Intellect()
    {
        OnGameOver += GameOver;
    }

    private void GameOver()
    {
        _isGameOver = true;
    }

    public virtual void AddCharacter(Character character)
    {
        character.OnAction += OnCharacterAction;
        _controlledCharacters.Add(character);
    }

    public virtual void RemoveCharacter(Character character)
    {
        character.OnAction -= OnCharacterAction;
        _controlledCharacters.Remove(character);
    }

    protected virtual void SellOnNeedMoney(Character character, int needMoney)
    {
        //
    }

    protected virtual void OnCharacterAction(CharacterActionType actionType, Character character)
    {
        if (_isGameOver) return;
        if (actionType == CharacterActionType.StartTurn)
        {
            character.Hold();
        }
        else if (actionType == CharacterActionType.EndHold)
        {
            character.CanFlip = true;
        }
        else if (actionType == CharacterActionType.EndFlip)
        {
            character.CanMove = true;
        }
        else if (actionType == CharacterActionType.TryPay)
        {
            int needMoney = _moneyCountingSystem.TryPay(character);
            if (needMoney == 0) character.Resume();
            else if (needMoney > 0) SellOnNeedMoney(character, needMoney);
            else
            {
                character.Dead(); 
            }
        }
        else if (actionType == CharacterActionType.EndMove)
        {
            if (character.TurnCounter >= character.CountTurnBeforeShooting)
            {
                character.CanShoot = true;
            }
            else
            {
                OnCharacterAction(CharacterActionType.EndShoot, character);
            }
        }
        else if (actionType == CharacterActionType.EndShoot)
        {
            if (character.IsDouble)
            {
                character.CanFlip = true;
            }
            else
            {
                character.CanEndTurn = true;
            }
        }
        else if (actionType == CharacterActionType.Arrest)
        {
            EndTurn(character);
        }
        else if (actionType == CharacterActionType.Dead)
        {
            if (CharactersData.GetCharacters.Count == 1)
            {
                OnGameOver?.Invoke();
                _gameOverPanel.Show();
                return;
            }
            

            _moneyCountingSystem.SellOfEntireState(character);
            character.OnAction -= OnCharacterAction;
            if (character.IsMyTurn) OnEndTurn?.Invoke(false);
        }
    }

    protected virtual void EndTurn(Character character)
    {
        if(character != null) character.EndTurn();
        OnEndTurn?.Invoke(true);
    }

    public virtual void Dispose()
    {
        for(int i = 0; i < _controlledCharacters.Count; i++) _controlledCharacters[i].OnAction -= OnCharacterAction;
        Intellect.OnGameOver -= GameOver;
    }

    public delegate void EndTurnDelegate(bool changeId);
    public static event EndTurnDelegate OnEndTurn;

    public delegate void GameOverDelegate();
    public static event GameOverDelegate OnGameOver;


}
