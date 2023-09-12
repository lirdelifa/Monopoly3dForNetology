using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InputListener : MonoBehaviour
{
    [SerializeField] private CameraMovement _cameraMovement;
    

    private void Update()
    {
        if(Input.GetMouseButton(1))
        {
            float horizontalAxis = Input.GetAxis("Horizontal");
            float verticalAxis = Input.GetAxis("Vertical");

            float MouseXAxis = Input.GetAxis("Mouse X");
            float MouseYAxis = Input.GetAxis("Mouse Y");

            _cameraMovement.Move(verticalAxis, horizontalAxis);
            _cameraMovement.Rotate(MouseYAxis, MouseXAxis);
        }

        if(Input.GetMouseButtonDown(0)) 
        {
            OnDownMouse0?.Invoke();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnUpMouse0?.Invoke();
        }
    }

    public delegate void DownMouse0();
    public static event DownMouse0 OnDownMouse0;

    public delegate void UpMouse0();
    public static event UpMouse0 OnUpMouse0;
}
