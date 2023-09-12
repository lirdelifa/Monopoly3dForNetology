using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AllSaveParameters
{
    public List<CharacterSaveParameters> charactersSaveParameters;
    public List<ProtectionWallSaveParameters> protectionWallSaveParameters;
    public List<PropertyCellSaveParameters> propertyCellSaveParameters;
    public bool isNewGame;
}
