using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CharacterSaveParameters
{
    public bool isBotControlled;
    public int health;
    public string name;
    public int cash;
    public int turnCounter;
    public bool isMyTurn;
    public int countTurnsInPrison;
    public float positionX;
    public float positionY;
    public float positionZ;
    public float rotationY;
}
