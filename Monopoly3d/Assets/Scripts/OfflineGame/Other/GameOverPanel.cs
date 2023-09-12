using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;

    public void Show()
    {
        gameObject.SetActive(true);
        _nameText.text = CharactersData.GetCharacters[0].Name + " גûידנאכ!";
    }
}
