using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private TMP_Text _botsCountText;
    [SerializeField] private DiceMenu _dice;
    private Vector3 _positionOfSelectedText;
    private Color _colorOfSelectedText;
    private Color _colorOfSelectedImage;
    private TMP_Text _selectedText;
    private GameObject _currentPanel;
    private int _currentBotsCount = 1;
    private const int _maxBotsCount = 5;

    private void OnEnable()
    {
        _dice.OnEndDiceAnimation += EndDiceAnimation;
    }

    private void OnDisable()
    {
        _dice.OnEndDiceAnimation -= EndDiceAnimation;
    }

    private void Start()
    {
        _currentPanel = _mainPanel;
    }

    private void EndDiceAnimation()
    {
        SceneManager.LoadScene("LoadScene");
    }

    public void SelectedButton(TMP_Text text)
    {
        _positionOfSelectedText = text.transform.position;
        text.transform.position -= new Vector3(_shiftSelectingText, 0, 0);
        _pistolImageTransform.gameObject.SetActive(true);
        Vector3 newPistolImageTransform = new Vector3(_pistolImageTransform.position.x, text.transform.position.y, 0);
        _pistolImageTransform.position = newPistolImageTransform;
        _colorOfSelectedText = text.color;
        text.color = Color.white;
        _selectedText = text;
    }

    public void DiselectedButton(TMP_Text text)
    {
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
        DiselectedButton(_selectedText);
        _currentPanel.SetActive(false);
        nextPanel.SetActive(true);
        _currentPanel = nextPanel;
    }

    public void DownExitButton()
    {
        Application.Quit();
    }

    public void DownNewOfflineGameButton()
    {
        _canvas.gameObject.SetActive(false);
        _dice.StartGoToLoadAnimation();
    }

    public void DownChangeBotsCountButton(int delta)
    {
        if (_currentBotsCount == 1 && delta == -1) return;
        if (_currentBotsCount == _maxBotsCount && delta == 1) return;

        _currentBotsCount += delta;
        _botsCountText.text = _currentBotsCount.ToString();
    }
}
