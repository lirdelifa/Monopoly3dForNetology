using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    [SerializeField] private CameraTransformation _camera;
    [SerializeField] private MouseClickHandler _mouseClickHandler;
    //[SerializeField] private LayerMask _layerMask;

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            float axisVertical = Input.GetAxis("Vertical");
            float axisHorixontal = Input.GetAxis("Horizontal");
            float axisMouseX = Input.GetAxis("Mouse X");
            float axisMouseY = Input.GetAxis("Mouse Y");
            float axisMouseWheel = Input.GetAxis("Mouse ScrollWheel");
            _camera.Move(axisVertical, axisHorixontal);
            _camera.Rotate(axisMouseX, -axisMouseY);
            _camera.Scroll(axisMouseWheel);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //_mouseClickHandler.OnClick();
           
        }
    }
}
