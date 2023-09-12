using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGamePanel : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private SaveLine _saveLinePrefab;
    [SerializeField] private RectTransform _content;
    [SerializeField] private Menu _menu;
    private bool _isInstantiateSaveLines;

    private SaveLine _currentSaveLine;

    private void Start()
    {
        _startGameButton.onClick.AddListener(StartGame);
    }

    public void SelectSaveLine(SaveLine saveLine)
    {
        if (_currentSaveLine != null) _currentSaveLine.Deselect();
        _currentSaveLine = saveLine;
    }


    private void OnEnable()
    {
        _currentSaveLine = null;
        if (_isInstantiateSaveLines) return;

        List<string> names = SaveManager.GetSaveNames();
        for(int i = 0; i < names.Count; i++)
        {
            SaveLine saveLine = Instantiate(_saveLinePrefab, _content).GetComponent<SaveLine>();
            saveLine.SetName(names[i]);
            saveLine.SetLoadGamePanel(this);
        }

        _isInstantiateSaveLines = true;
    }

    private void StartGame()
    {
        if (_currentSaveLine == null) return;
        SaveManager.SetCurrentAllSaveParameters(_currentSaveLine.GetName);
        _menu.DownLoadGameButton();
    }

    public void DeleteSave(SaveLine saveLine)
    {
        if(_currentSaveLine == saveLine) _currentSaveLine = null;
        SaveManager.DeleteSave(saveLine.GetName);
    }
}
