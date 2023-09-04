using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MouseClickHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _layerMask;
    private UnitMove _unitMove;
    private GameObject _selectedObject;
    void Start()
    {
        
    }

    public void SetUnit(UnitMove unitMove)
    {
        _unitMove = unitMove;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f, _layerMask))
        {
            //if (hit.collider.TryGetComponent<Outline>(out Outline outline))
            //{
                //if(_selectedObject != null) _selectedObject.GetComponent<Outline>().enabled = false;
                //outline.enabled = true;
               //_selectedObject = outline.gameObject;
            //}

            if (hit.collider.TryGetComponent<TargetPoint>(out TargetPoint point))
            {
                if(_unitMove != null)
                {
                    _unitMove.MovingTo(point.GetPoint, point.GetCell.Id);
                }
                //print(point.GetPoint);
            }


            // Do something with the object that was hit by the raycast.
        }
    }
}
