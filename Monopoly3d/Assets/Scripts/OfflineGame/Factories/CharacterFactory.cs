using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterFactory
{
    private DiContainer _container;
    private const string _characterName = "Character";
    private Object _characterObject;
    CharacterFactory(DiContainer container) => _container = container;

    public void Load()
    {
        _characterObject = Resources.Load(_characterName);
    }

    public Character Create(Vector3 position)
    {
        Character character = _container.InstantiatePrefab(_characterObject, position, Quaternion.identity, null).GetComponent<Character>();
        CharactersData.AddCharacter(character);
        return character;
    }
}
