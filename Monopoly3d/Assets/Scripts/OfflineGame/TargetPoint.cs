using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TargetPoint : PointerHandler
{
    [SerializeField] private Cell _cell;
    [SerializeField] private Image _image;

    public Vector3 GetPoint { get => transform.position; }
    public Cell GetCell { get => _cell; }

    public bool IsFree = true;

    protected override void Awake()
    {
        //
    }

    protected override void Selected(bool isSelected)
    {
        if (_isSelected) return;
        if (_isPointerDown) return;
        //if ((_isSelected && !isSelected) || _isPointerDown) return;
        _image.color = new Color(0, 0, 0, (isSelected) ? 1 : 0);
    }

    public override void OnAction(ActionButtonType buttonType)
    {
        switch (buttonType)
        {
            case ActionButtonType.Select :
                _isSelected = !_isSelected;
                Selected(_isSelected);
                break;
            case ActionButtonType.MoveToPoint:
                _unitData.PlayerMove.MovingTo(GetPoint, GetCell.Id);
                break;
        }

    }


}
