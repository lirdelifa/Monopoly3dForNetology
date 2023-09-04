using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Inject] protected GUIHandler _GUIHandler;
    [Inject] protected UnitData _unitData;
    [SerializeField] protected ClickableHandlerType _clickableHandlerType;
    protected bool _isSelected = false;
    protected bool _isPointerDown = false;
    
    private Outline _outline;

    public ClickableHandlerType GetClickableHandlerType { get => _clickableHandlerType; }

    //private static PointerHandler _currentClicedObject;

    protected virtual void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Selected(true);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Selected(false);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        _isPointerDown = true;
        Selected(true);
        _GUIHandler.ShowActionCircle(this);
    }

    protected virtual void Selected(bool isSelected)
    {
        //if ((_isSelected && !isSelected) || _isPointerDown) return;
        //if (_outline != null) _outline.enabled = isSelected;


        if (_isSelected) return;
        if (_isPointerDown) return;
        if (_outline != null) _outline.enabled = isSelected;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        _GUIHandler.HideActionCircle();
        _isPointerDown = false;
        Selected(false);
    }

    public virtual void OnAction(ActionButtonType buttonType)
    {
        switch (buttonType)
        {
            case ActionButtonType.Select:
                _isSelected = !_isSelected;
                Selected(_isSelected);
                break;
        }
    }
}
