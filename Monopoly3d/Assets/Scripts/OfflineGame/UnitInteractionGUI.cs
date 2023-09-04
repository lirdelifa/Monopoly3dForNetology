using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Outline))]
public class UnitInteractionGUI : PointerHandler
{
    public override void OnAction(ActionButtonType buttonType)
    {
        switch (buttonType)
        {
            case ActionButtonType.Select:
                _isSelected = !_isSelected;
                Selected(_isSelected);
                break;
            case ActionButtonType.CameraRotatingAround:
                _unitData.CameraTransformation.RotateAroundObject(transform);
                break;
        }
    }
}
