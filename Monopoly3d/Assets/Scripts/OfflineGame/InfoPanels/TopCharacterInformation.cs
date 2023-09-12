using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class TopCharacterInformation : MonoBehaviour
{
    [SerializeField] private TMP_Text _characterNameText;
    [SerializeField] private TMP_Text _characterCashText;
    [SerializeField] private TMP_Text _characterMonetaryConditionText;
    [SerializeField] private GameObject _canFlipImage;
    [SerializeField] private GameObject _canShootImage;
    [SerializeField] private Button _endTurnButton;

    private void Start()
    {
        _endTurnButton.onClick.AddListener(DownEndTurnButton);
    }

    public void Show(Character character)
    {
        gameObject.SetActive(true);
        _characterNameText.text = character.Name;
        _characterCashText.text = character.Cash.ToString() + "$";
        _characterMonetaryConditionText.text = character.MonetaryCondition.ToString() + "$";

        _canFlipImage.SetActive(character.CanFlip);
        _canShootImage.SetActive(character.CanShoot);
        _endTurnButton.gameObject.SetActive(character.CanEndTurn);
    }

    private void DownEndTurnButton()
    {
        OnEndTurn?.Invoke();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public delegate void EndTurnDelegate();
    public static event EndTurnDelegate OnEndTurn;
}
