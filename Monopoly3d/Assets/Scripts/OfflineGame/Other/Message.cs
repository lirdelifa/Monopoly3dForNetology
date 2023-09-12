using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Message : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void SetText(string text) => _text.text = text;
}
