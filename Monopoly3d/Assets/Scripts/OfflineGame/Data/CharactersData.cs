using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersData : IDispose
{
    private static List<Character> _characters = new List<Character>();
    public static List<Character> GetCharacters { get => _characters; }

    CharactersData() 
    {

    }

    public static void AddCharacter(Character character) => _characters.Add(character);

    public static int GetCountCharacters { get => _characters.Count; }

    public static List<Character> GetCharactersExcept(Character character)
    {
        List<Character> characters = new List<Character>();
        for(int i = 0; i < _characters.Count; i++)
        {
            if (_characters[i] != character) characters.Add(_characters[i]);
        }
        return characters;
    }

    public static void RemoveCharacter(Character character) => _characters.Remove(character);

    public void Dispose()
    {
        _characters = new List<Character>();
    }
}
