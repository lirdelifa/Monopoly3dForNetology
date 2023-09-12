using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRaycasterListener
{
    public List<ButtonActionType> GetButtonActionTypes { get; }
    public Vector3 GetPoint { get; }

    public void ChangeButtonActionTypes(List<ButtonActionType> buttonActionTypes);
}
