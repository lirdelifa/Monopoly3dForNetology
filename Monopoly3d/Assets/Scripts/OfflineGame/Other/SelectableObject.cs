using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(QuickOutline))]
public class SelectableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IRaycasterListener, IDamagable
{
    private QuickOutline _quickOutline;
    private bool _selected = false;

    private List<ButtonActionType> _buttonActionTypes;
    public List<ButtonActionType> GetButtonActionTypes { get => _buttonActionTypes; }
    public Vector3 GetPoint { get => transform.position; }
    public void ChangeButtonActionTypes(List<ButtonActionType> buttonActionTypes) => _buttonActionTypes = buttonActionTypes;


    protected virtual void Start()
    {
        _quickOutline = GetComponent<QuickOutline>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _quickOutline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_selected) return;
        _quickOutline.enabled = false;
    }

    protected virtual void OnEnable()
    {
        ActionCircle.OnSelectObject += Select;
    }

    protected virtual void OnDisable()
    {
        ActionCircle.OnSelectObject -= Select;
    }

    private void Select(IRaycasterListener raycasterListener)
    {
        if (raycasterListener != (IRaycasterListener)this) return;
        _selected = !_selected;
        _quickOutline.enabled = _selected;
    }

    public virtual void TakeDamage(int damage, Vector3 direction)
    {
        //
    }
}
