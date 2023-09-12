using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionCircle : MonoBehaviour
{
    [SerializeField] private List<ButtonActionConfig> _buttonActionConfigs;
    [SerializeField] private List<ButtonAction> _buttonsAction;
    [SerializeField] private Image _loadCircleImage;
    [SerializeField] private GameObject _mainCircle;
    [SerializeField] private float _timeForLoadCircleAnimation;

    private Coroutine _currentLoadCircleAnimation = null;
    private IRaycasterListener _currentRaycastListener;
    private ButtonActionType _currentButtonActionType = ButtonActionType.None;

    private void Show(IRaycasterListener raycasterListener)
    {
        if (_currentLoadCircleAnimation != null) StopCoroutine(_currentLoadCircleAnimation);
        _currentLoadCircleAnimation = StartCoroutine(LoadCircleAnimation());
        _currentRaycastListener = raycasterListener;
    }

    public void Hide()
    {
        if (_currentButtonActionType != ButtonActionType.None) DefiningAnAction();
        if (_currentLoadCircleAnimation != null) StopCoroutine(_currentLoadCircleAnimation);
        _mainCircle.SetActive(false);
        _loadCircleImage.gameObject.SetActive(false);
        _currentRaycastListener = null;
        _currentButtonActionType = ButtonActionType.None;
    }

    public void SetCurrentButtonActionType(ButtonActionType buttonActionType)
    {
        if(buttonActionType == _currentButtonActionType) _currentButtonActionType = ButtonActionType.None;
        else _currentButtonActionType = buttonActionType;
    }

    private void DefiningAnAction()
    {
        if (_currentButtonActionType == ButtonActionType.Move) OnMoveToPoint?.Invoke(_currentRaycastListener.GetPoint);
        else if (_currentButtonActionType == ButtonActionType.AddCharacter) OnTransferCharacter?.Invoke(_currentRaycastListener, true);
        else if (_currentButtonActionType == ButtonActionType.RemoveCharacter) OnTransferCharacter?.Invoke(_currentRaycastListener, false);
        else if (_currentButtonActionType == ButtonActionType.Select) OnSelectObject?.Invoke(_currentRaycastListener);
        else if (_currentButtonActionType == ButtonActionType.Info) OnInfo?.Invoke(_currentRaycastListener);
        else if (_currentButtonActionType == ButtonActionType.Flip) OnFlip?.Invoke();
        else if (_currentButtonActionType == ButtonActionType.Shoot) OnShoot?.Invoke(_currentRaycastListener.GetPoint);
        else if (_currentButtonActionType == ButtonActionType.Buy) OnBuy?.Invoke(_currentRaycastListener);
        else if (_currentButtonActionType == ButtonActionType.Sell) OnSell?.Invoke(_currentRaycastListener);
    }

    private IEnumerator LoadCircleAnimation()
    {
        _loadCircleImage.gameObject.SetActive(true);
        PositioningRelativeToScreen(_loadCircleImage.GetComponent<RectTransform>());

        float timer = 0;

        while(timer < _timeForLoadCircleAnimation)
        {
            
            timer += Time.deltaTime;
            _loadCircleImage.fillAmount = timer / _timeForLoadCircleAnimation;
            yield return null;
        }

        _loadCircleImage.gameObject.SetActive(false);
        _mainCircle.SetActive(true);
        PositioningRelativeToScreen(_mainCircle.GetComponent<RectTransform>());
        _currentLoadCircleAnimation = null;
        ShowButton();
    }

    private void ShowButton()
    {
        for(int i = 0; i < _buttonsAction.Count; i++)
        {
            if(i < _currentRaycastListener.GetButtonActionTypes.Count)
            {
                _buttonsAction[i].Show(true, GetConfigByType(_currentRaycastListener.GetButtonActionTypes[i]));
            }
            else
            {
                _buttonsAction[i].Show(false, new ButtonActionConfig());
            }
        }
    }

    private ButtonActionConfig GetConfigByType(ButtonActionType buttonActionType)
    {
        for(int i = 0; i < _buttonActionConfigs.Count; i++)
        {
            if (_buttonActionConfigs[i].buttonActionType == buttonActionType) return _buttonActionConfigs[i];
        }

        return new ButtonActionConfig();
    }

    private void PositioningRelativeToScreen(RectTransform rectTransform)
    {
        float mouseX = Input.mousePosition.x - Screen.width / 2;
        float mouseY = Input.mousePosition.y - Screen.height / 2;

        float maxX = Screen.width / 2 - rectTransform.sizeDelta.x / 2;
        float maxY = Screen.height / 2 - rectTransform.sizeDelta.y / 2;
        mouseX = Mathf.Clamp(mouseX, -maxX, maxX);
        mouseY = Mathf.Clamp(mouseY, -maxY, maxY);

        rectTransform.anchoredPosition = new Vector3(mouseX, mouseY, 0f);
    }

    private void OnEnable()
    {
        Raycaster.OnRaycastHit += Show;
        InputListener.OnUpMouse0 += Hide;
    }

    private void OnDisable()
    {
        Raycaster.OnRaycastHit -= Show;
        InputListener.OnUpMouse0 -= Hide;
    }

    public delegate void MoveToPoint(Vector3 point);
    public static MoveToPoint OnMoveToPoint;

    public delegate void TransferCharacter(IRaycasterListener character, bool adding);
    public static TransferCharacter OnTransferCharacter;

    public delegate void SelectObject(IRaycasterListener character);
    public static SelectObject OnSelectObject;

    public delegate void InfoDelegate(IRaycasterListener character);
    public static InfoDelegate OnInfo;

    public delegate void FlipDelegate();
    public static FlipDelegate OnFlip;

    public delegate void ShootDelegate(Vector3 point);
    public static ShootDelegate OnShoot;

    public delegate void BuyDelegate(IRaycasterListener raycasterListener);
    public static BuyDelegate OnBuy;

    public delegate void SellDelegate(IRaycasterListener raycasterListener);
    public static SellDelegate OnSell;
}
