using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : IDispose
{
    public static AllSaveParameters AllParameters { get; private set; }

    SaveSystem()
    {
        Character.OnChangeSaveParameters += ChangeAllSaveParameters;
    }

    public void Dispose()
    {
        Character.OnChangeSaveParameters -= ChangeAllSaveParameters;
    }

    private void ChangeAllSaveParameters()
    {
        AllParameters = new AllSaveParameters();
        AllParameters.isNewGame = false;
        AllParameters.charactersSaveParameters = new List<CharacterSaveParameters>();
        AllParameters.propertyCellSaveParameters = new List<PropertyCellSaveParameters>();
        AllParameters.protectionWallSaveParameters = new List<ProtectionWallSaveParameters>();

        for(int i = 0; i < CharactersData.GetCharacters.Count; i++)
        {
            AllParameters.charactersSaveParameters.Add(CharactersData.GetCharacters[i].GetSaveParameters());
        }
        for (int i = 0; i < CellsData.GetCellProperties().Count; i++)
        {
            CellProperty cellProperty = CellsData.GetCellProperties()[i];
            AllParameters.propertyCellSaveParameters.Add(cellProperty.GetSaveParameters());
            for (int b = 0; b < cellProperty.GetProtectionWalls.Count; b++) AllParameters.protectionWallSaveParameters.Add(cellProperty.GetProtectionWalls[b].GetSaveParameters());
        }
    }
}
