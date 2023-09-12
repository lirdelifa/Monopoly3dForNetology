using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

public class SaveManager
{
    private static AllSaveParameters _currentAllSaveParameters;

    private static int _delfaultCountCharactersForNewGame = 3;
    private static string _directoryName = Application.persistentDataPath + "/SavedGames";
    private static string _DATExtensionName = ".dat";

    public static AllSaveParameters GetCurrentAllSaveParameters()
    {
        if (_currentAllSaveParameters == null) return GetAllSaveParametersForNewGame(_delfaultCountCharactersForNewGame);
        else return _currentAllSaveParameters;
    }

    public static void SetCurrentAllSaveParameters(string saveName)
    {
        if (File.Exists(_directoryName + "/" + saveName + _DATExtensionName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
              File.Open(_directoryName + "/" + saveName + _DATExtensionName, FileMode.Open);
            _currentAllSaveParameters = (AllSaveParameters)bf.Deserialize(file);
            file.Close();
        }
        else SetCurrentAllSaveParameters(_delfaultCountCharactersForNewGame);
    }
    public static void SetCurrentAllSaveParameters(int countCharacters)
    {
        _currentAllSaveParameters = GetAllSaveParametersForNewGame(countCharacters);
    }

    public static void DeleteSave(string saveName)
    {
        if (File.Exists(_directoryName + "/" + saveName + _DATExtensionName))
        {
            File.Delete(_directoryName + "/" + saveName + _DATExtensionName);
        }
    }

    private static AllSaveParameters GetAllSaveParametersForNewGame(int countCharacters)
    {
        AllSaveParameters allParameters;

        allParameters = new AllSaveParameters();
        allParameters.charactersSaveParameters = new List<CharacterSaveParameters>();
        allParameters.propertyCellSaveParameters = new List<PropertyCellSaveParameters>();
        allParameters.protectionWallSaveParameters = new List<ProtectionWallSaveParameters>();
        allParameters.isNewGame = true;

        for(int i = 0; i < countCharacters; i++)
        {
            CharacterSaveParameters parameters = new CharacterSaveParameters();

            parameters.isBotControlled = i != 0;
            parameters.health = 100;
            parameters.name = "Игрок " + (i + 1).ToString();
            parameters.cash = 1250;
            parameters.turnCounter = 0;
            parameters.isMyTurn = i == 0;
            parameters.countTurnsInPrison = 0;
            parameters.rotationY = 0;

            allParameters.charactersSaveParameters.Add(parameters);
        }


        return allParameters;
    }

    public static bool TrySaveGame(AllSaveParameters allSaveParameters, string saveName)
    {
        if (GetSaveNames().Contains(saveName)) return false;

        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(_directoryName + "/" + saveName + _DATExtensionName);
        bf.Serialize(file, allSaveParameters);
        file.Close();
        return true;
    }

    public static List<string> GetSaveNames()
    {
        List<string> names = new List<string>();

        if(Directory.Exists(_directoryName))
        {
            names = Directory.GetFiles(_directoryName).ToList();
            for (int i = 0; i < names.Count; i++)
            {
                names[i] = names[i].Substring(_directoryName.Length + 1);
                names[i] = names[i].Substring(0, names[i].Length - _DATExtensionName.Length);
            }
        }


        return names;
    }
}
