using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveGamePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _notificationText;
    [SerializeField] private Color _notificationTextTrueColor;
    [SerializeField] private Color _notificationTextFalseColor;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _saveButton;

    private const int _minLenghtSaveName = 4;

    private void Start()
    {
        _backButton.onClick.AddListener(Close);
        _saveButton.onClick.AddListener(SaveGame);
    }

    private void OnEnable()
    {
        _notificationText.gameObject.SetActive(false);
        _inputField.text = string.Empty;
    }

    public void Show() => gameObject.SetActive(true);

    private void Close() => gameObject.SetActive(false);

    private void SaveGame()
    {
        if (CharactersData.GetCountCharacters == 1)
        {
            _notificationText.gameObject.SetActive(true);
            _notificationText.color = _notificationTextFalseColor;
            _notificationText.text = "Игра уже окончена!";
            return;
        }

        string saveName = _inputField.text;
        saveName = saveName.Trim();
        saveName = saveName.Replace(" ", "_");
        
        if(saveName.Length < _minLenghtSaveName)
        {
            _notificationText.gameObject.SetActive(true);
            _notificationText.color = _notificationTextFalseColor;
            _notificationText.text = "Минимальное имя сохранения должно быть >= " + _minLenghtSaveName.ToString();
            return;
        }

        _notificationText.gameObject.SetActive(true);

        if (SaveManager.TrySaveGame(SaveSystem.AllParameters, saveName))
        {
            
            _notificationText.color = _notificationTextTrueColor;
            _notificationText.text = "Игра \"" + saveName + "\" сохранена!";
        }
        else
        {
            _notificationText.color = _notificationTextFalseColor;
            _notificationText.text = "Сохранение \"" + saveName + "\" уже существует!";
        }
    }
}
