using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetPoint : MonoBehaviour, IRaycasterListener, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private List<ButtonActionType> _buttonActionTypes;
    [SerializeField] private GameObject _circle;
    public bool IsFree { get; set; } = true;

    public List<ButtonActionType> GetButtonActionTypes { get => _buttonActionTypes; }
    public Vector3 GetPoint { get => transform.position; }
    public void ChangeButtonActionTypes(List<ButtonActionType> buttonActionTypes) => _buttonActionTypes = buttonActionTypes;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _circle.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _circle.SetActive(false);
    }
}
