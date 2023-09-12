using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] Canvas _canvas;
    [SerializeField] private Transform _pistolImageTransform;
    [SerializeField] private float _shiftSelectingText;
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _loadGamePanel;
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private TMP_Text _botsCountText;
    [SerializeField] private DiceMenu _dice;
    [SerializeField] private AudioSource _audioSource;
    private Vector3 _positionOfSelectedText;
    private Color _colorOfSelectedText;
    private Color _colorOfSelectedImage;
    private TMP_Text _selectedText;
    private GameObject _currentPanel;
    private int _currentBotsCount = 2;
    private const int _maxBotsCount = 6;
    private bool _lockedButtons;

    private void OnEnable()
    {
        _dice.OnEndDiceAnimation += EndDiceAnimation;
        SettingsPanel.OnChangeEnvironmentVolume += ChangeVolume;
    }

    private void OnDisable()
    {
        _dice.OnEndDiceAnimation -= EndDiceAnimation;
        SettingsPanel.OnChangeEnvironmentVolume -= ChangeVolume;
    }

    private void ChangeVolume(float newVolume)
    {
        _audioSource.volume = newVolume;
    }

    private void Start()
    {
        _currentPanel = _mainPanel;
        if (PlayerPrefs.HasKey(SettingsPanel.EnvironmentVolumePPName)) _audioSource.volume = PlayerPrefs.GetFloat(SettingsPanel.EnvironmentVolumePPName);
        else _audioSource.volume = SettingsPanel.DefaultEnvironmentVolume;
    }

    private void EndDiceAnimation()
    {
        SceneManager.LoadScene("LoadScene");
    }

    public void SelectedButton(TMP_Text text)
    {
        if (_lockedButtons) return;
        _positionOfSelectedText = text.transform.position;
        text.transform.position -= new Vector3(_shiftSelectingText, 0, 0);
        _pistolImageTransform.gameObject.SetActive(true);
        Vector3 newPistolImageTransform = new Vector3(_pistolImageTransform.position.x, text.transform.position.y, 0);
        _pistolImageTransform.position = newPistolImageTransform;
        _colorOfSelectedText = text.color;
        text.color = Color.white;
        _selectedText = text;
        _audioSource.Play();
    }

    public void DiselectedButton(TMP_Text text)
    {
        if (_lockedButtons) return;
        text.color = _colorOfSelectedText;
        text.transform.position = _positionOfSelectedText;
        _pistolImageTransform.gameObject.SetActive(false);
    }

    public void SelectedImageButton(Image image)
    {
        _colorOfSelectedImage = image.color;
        image.color = Color.white;
    }

    public void DiselectedImageButton(Image image)
    {
        image.color = _colorOfSelectedImage;
    }

    public void DownButton(GameObject nextPanel)
    {
        if (_lockedButtons) return;
        DiselectedButton(_selectedText);
        _currentPanel.SetActive(false);
        nextPanel.SetActive(true);
        _currentPanel = nextPanel;
    }

    public void DownExitButton()
    {
        Application.Quit();
    }

    public void DownSettingsButton()
    {
        _settingsPanel.Show();
    }

    public void DownNewOfflineGameButton()
    {
        if (_lockedButtons) return;
        SaveManager.SetCurrentAllSaveParameters(_currentBotsCount);
        StartGame();
    }

    public void DownLoadGameButton()
    {
        StartGame();
    }

    private void StartGame()
    {
        _canvas.gameObject.SetActive(false);
        _settingsPanel.gameObject.SetActive(false);
        _dice.StartGoToLoadAnimation();
    }

    public void DownChangeBotsCountButton(int delta)
    {
        if (_currentBotsCount == 2 && delta == -1) return;
        if (_currentBotsCount == _maxBotsCount && delta == 1) return;

        _currentBotsCount += delta;
        _botsCountText.text = _currentBotsCount.ToString();
    }

    public void EnableLoadGamePanel(bool isEnable)
    {
        if(_selectedText != null) DiselectedButton(_selectedText);
        _lockedButtons = isEnable;
        _loadGamePanel.SetActive(isEnable);
    }
}
