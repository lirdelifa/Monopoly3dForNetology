using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionCircle : MonoBehaviour
{
    [SerializeField] private List<ActionCircleButton> _buttons;
    private ActionCircleButton _currentButton;

    [SerializeField] private List<ActionButtonConfig> _pointConfig;
    [SerializeField] private List<ActionButtonConfig> _unitConfig;
    [SerializeField] private List<ActionButtonConfig> _protectionConfig;

    [SerializeField] private Color _lockedColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _deselectedColor;
    [SerializeField] private float _delta;
    [SerializeField] private float _minRaycastPadding;
    [SerializeField] private float _maxRaycastPadding;

    private PointerHandler _currentPointerHandler;

    public void ChangeButton(ActionCircleButton button, bool isSet)
    {
        if (isSet) _currentButton = button;
        else if (_currentButton != button) return;
        else _currentButton = null;
    }


    public void Show(PointerHandler pointerHandler)
    {
        gameObject.SetActive(true);
        _currentPointerHandler = pointerHandler;
        switch (pointerHandler.GetClickableHandlerType) 
        {
            case ClickableHandlerType.Unit:
                SetButtonsParameters(_unitConfig);
                break;
            case ClickableHandlerType.Protection:
                SetButtonsParameters(_protectionConfig);
                break;
            case ClickableHandlerType.Point:
                SetButtonsParameters(_pointConfig);
                break;
        }

    }

    private void SetButtonsParameters(List<ActionButtonConfig> config)
    {
        for(int i =  0; i < _buttons.Count; i++) 
        {
            _buttons[i].Delta = _delta;
            _buttons[i].LockedColor = _lockedColor;
            _buttons[i].SelectedColor = _selectedColor;
            _buttons[i].DeselectedColor = _deselectedColor;
            _buttons[i].MinRaycastPadding = _minRaycastPadding;
            _buttons[i].MaxRaycastPadding = _maxRaycastPadding;
            if(i < config.Count)
            {
                _buttons[i].IsLocked = false;
                _buttons[i].SetIcon(config[i].IconPrefab);
                _buttons[i].ButtonType = config[i].ButtonType;
            }
            else
            {
                _buttons[i].IsLocked = true;
            }
            _buttons[i].Show();
        }
    }


    public void Hide()
    {
        if (_currentButton != null)
        {
            _currentPointerHandler.OnAction(_currentButton.ButtonType);
        }
        _currentButton = null;
        gameObject.SetActive(false);
    }
}
