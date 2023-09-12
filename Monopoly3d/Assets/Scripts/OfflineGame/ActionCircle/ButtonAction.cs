using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform _iconParent;
    [SerializeField] private Image _image;

    [SerializeField] private Color _lockedColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _deselectedColor;
    [SerializeField] private ActionCircle _actionCircle;

    private bool _isEnabled = false;
    private GameObject _currentIcon;
    public ButtonActionType ButtonActionType { get; private set; }

    private const float AlphaHitTestMinimumThreshold = 1f;

    private void Start()
    {
        _image.alphaHitTestMinimumThreshold = AlphaHitTestMinimumThreshold;
    }


    public void Show(bool enable, ButtonActionConfig config)
    {
        _isEnabled = enable;
        if (_isEnabled)
        {
            _currentIcon = Instantiate(config.iconPrefab, _iconParent);
            ButtonActionType = config.buttonActionType;
            _image.color = _deselectedColor;
        }
        else _image.color = _lockedColor;
        
    }

    private void OnDisable()
    {
        if(_currentIcon != null) Destroy(_currentIcon);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isEnabled) return;
        _image.color = _selectedColor;
        _actionCircle.SetCurrentButtonActionType(ButtonActionType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isEnabled) return;
        _image.color = _deselectedColor;
        _actionCircle.SetCurrentButtonActionType(ButtonActionType);
    }
}
