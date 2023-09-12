using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicesFlipSystem : IDispose
{
    private List<Dice> _dices = new List<Dice>();
    private Character _currentCharacter;

    public void AddAllDices(List<Dice> dices) => _dices = dices;

    DicesFlipSystem()
    {
        Dice.OnEndFlip += EndDiceFlip;
        Character.OnFlip += Flip;
    }   
    
    private void Flip(Character character)
    {
        if (!character.IsMyTurn) return;
        _currentCharacter = character;
        for (int i = 0; i < _dices.Count; i++)
        {
            _dices[i].Flip();
        }
    }

    private void EndDiceFlip()
    {
        int fellOutNumber = 0;
        bool isDouble = _dices.Count > 1;
        int lastNumber = 0;

        for (int i = 0; i < _dices.Count; i++) 
        {
            if (_dices[i].IsEndFlip == false) return;
            fellOutNumber += _dices[i].FellOutNumber;
            if (i != 0 & _dices[i].FellOutNumber != lastNumber) isDouble = false;
            lastNumber = _dices[i].FellOutNumber;
        }

        if (isDouble) Debug.Log("Дубль!");

        _currentCharacter.EndFlip(fellOutNumber, isDouble);
    }

    public void Dispose()
    {
        Dice.OnEndFlip -= EndDiceFlip;
        Character.OnFlip -= Flip;
    }
}
