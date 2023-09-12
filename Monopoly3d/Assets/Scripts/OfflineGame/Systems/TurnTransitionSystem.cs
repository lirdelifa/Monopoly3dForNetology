using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTransitionSystem : IDispose
{
    TurnTransitionSystem()
    {
        Intellect.OnEndTurn += EndTurn;
    }

    private int _currentCharacterId;

    public void SetCurrentCharacterId(int id) => _currentCharacterId = id;

    public void NextTurn(bool changeId)
    {
        if(changeId)
        {
            _currentCharacterId = (_currentCharacterId == CharactersData.GetCountCharacters - 1) ? 0 : _currentCharacterId + 1;
        }

        OnStartTurn?.Invoke(CharactersData.GetCharacters[_currentCharacterId]);
        CharactersData.GetCharacters[_currentCharacterId].StartTurn();
    }

    private void EndTurn(bool changeId) => NextTurn(changeId);

    public void Dispose()
    {
        Intellect.OnEndTurn -= EndTurn;
    }

    public delegate void StartTurn(Character character);
    public static event StartTurn OnStartTurn;
}
