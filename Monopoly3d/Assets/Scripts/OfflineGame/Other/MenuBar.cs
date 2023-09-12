using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuBar : MonoBehaviour
{
    private List<IDispose> _disposes = new List<IDispose>();

    [SerializeField] private Button _menuButton;
    [SerializeField] private GameObject _buttonsPanel;
    [SerializeField] private Button _leaveButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _confirmLeaveButton;
    [SerializeField] private Button _backButton;

    [SerializeField] private Sprite _pauseSprite;
    [SerializeField] private Sprite _playSprite;

    [SerializeField] private GameObject _warningLeavePanel;
    [SerializeField] private SaveGamePanel _saveGamePanel;
    [SerializeField] private SettingsPanel _settingsPanel;

    private bool _isPause = false;

    public void SetDisposes(List<IDispose> disposes) => _disposes = disposes;

    private void Start()
    {
        _menuButton.onClick.AddListener(ShowButtonsPanel);
        _pauseButton.onClick.AddListener(Pause);
        _leaveButton.onClick.AddListener(ShowWarningLeavePanel);
        _backButton.onClick.AddListener(ShowWarningLeavePanel);
        _confirmLeaveButton.onClick.AddListener(Leave);
        _saveButton.onClick.AddListener(Save);
        _settingsButton.onClick.AddListener(Settings);
    }

    private void ShowButtonsPanel()
    {
        _buttonsPanel.SetActive(!_buttonsPanel.activeSelf);
    }

    private void Pause()
    {
        _isPause = !_isPause;
        if (_isPause)
        {
            _pauseButton.image.sprite = _playSprite;
            Time.timeScale = 0f;
        }       
        else
        {
            _pauseButton.image.sprite = _pauseSprite;
            Time.timeScale = 1f;
        }
    }

    private void ShowWarningLeavePanel()
    {
        _warningLeavePanel.SetActive(!_warningLeavePanel.activeSelf);
    }

    private void Leave()
    {
        Time.timeScale = 1f;
        for (int i = 0; i < _disposes.Count; i++) _disposes[i].Dispose();
        SceneManager.LoadScene(Load.MenuSceneName);
    }

    private void Save()
    {
        _saveGamePanel.Show();
    }

    private void Settings()
    {
        _settingsPanel.Show();
    }
}
