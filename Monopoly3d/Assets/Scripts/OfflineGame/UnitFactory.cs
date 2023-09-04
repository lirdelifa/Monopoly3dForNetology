using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnitFactory
{
    private static DiContainer _diContainer;
    private static Object _botPrefab;
    private static Object _playertPrefab;

    private const string _botPrefabName = "Bot";
    private const string _playerPrefabName = "Player";

    private UnitFactory(DiContainer diContainer)
    {
        _diContainer = diContainer;
    }

    public void Load()
    {
        //_botPrefab = Resources.Load(_botPrefabName);
        _playertPrefab = Resources.Load(_playerPrefabName);
    }

    public static void Create(Vector3 position, Quaternion rotation)
    {
        _diContainer.InstantiatePrefab(_playertPrefab, position, rotation, null);
    }
}
