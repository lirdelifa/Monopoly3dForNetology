using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraTransformation : MonoBehaviour
{
    [Inject] private UnitData _unitData;
    [SerializeField, Range(0f, 100f)] private float _speedMove;
    [SerializeField, Range(0f, 200f)] private float _speedRotateHorizontal;
    [SerializeField, Range(0f, 200f)] private float _speedRotateVertical;
    [SerializeField, Range(0f, 200f)] private float _speedScroll;
    //[SerializeField] private float _maxRotateVertical;
    //[SerializeField] private float _minRotateVertical;
    [SerializeField, Range(0f, 85f)] private float _maxAngleRotation;
    private const float _maxDistanceByCenter = 100f;
    //private float _currentVerticalRotation;
    private bool _isRotateAroundObject;
    private Transform _rotateAroundObjectTransform;
    private Vector3 _currentDeltaPosition;
    private float _currentDistanceByCenter;

    //private float _currentZoom;
    private float _minZoom = 2f;
    private float _maxZoom = 50f;

    //private float _currentMaxDistanceByCenter;

    private void Start()
    {
        _unitData.SetCameraTransformation(this);

    }

    public void Move(float moveForward, float moveRight)
    {
        if (_isRotateAroundObject) return;
        Vector3 movement = (transform.forward * moveForward + transform.right * moveRight) * _speedMove * Time.deltaTime;
        transform.position += movement;
    }

    public void Rotate(float rotateY, float rotateX) 
    {


        /*_currentVerticalRotation += rotateX * _speedRotateVertical * Time.deltaTime;
        _currentVerticalRotation = Mathf.Clamp(_currentVerticalRotation, _minRotateVertical, _maxRotateVertical);

        float rotationHorizontal = transform.localEulerAngles.y + rotateY * _speedRotateHorizontal * Time.deltaTime;
        transform.rotation = Quaternion.Euler(_currentVerticalRotation, rotationHorizontal, 0);

        if(_isRotateAroundObject)
        {
            transform.position = _rotateAroundObjectTransform.position - transform.forward * ;
            _currentDeltaPosition = transform.position - _rotateAroundObjectTransform.position;
        }*/

        if (rotateX > 0 &&  90 - Vector3.Angle(transform.forward, -Vector3.up) > _maxAngleRotation) return;
        if (rotateX < 0 && 90 - Vector3.Angle(transform.forward, Vector3.up) > _maxAngleRotation) return;

        //print(rotateX + " || " + (90 - Vector3.Angle(transform.forward, Vector3.up)));

        if (_isRotateAroundObject)
        {
            //if (transform.rotation.eulerAngles.x >= _maxRotateVertical && rotateX < 0f) rotateX = 0f;
            //else if (transform.rotation.eulerAngles.x <= _minRotateVertical && rotateX > 0f) rotateX = 0f;
            //print(transform.rotation[0] * 120f);

            transform.RotateAround(_rotateAroundObjectTransform.position, Vector3.up, rotateY * _speedRotateHorizontal * Time.deltaTime);
            transform.RotateAround(_rotateAroundObjectTransform.position, transform.right, rotateX * _speedRotateVertical * Time.deltaTime);

            transform.LookAt(_rotateAroundObjectTransform);
            _currentDeltaPosition = transform.position - _rotateAroundObjectTransform.position;
        }
        else
        {
            //_currentVerticalRotation += rotateX * _speedRotateVertical * Time.deltaTime;
            //_currentVerticalRotation = Mathf.Clamp(_currentVerticalRotation, _minRotateVertical, _maxRotateVertical);

            float rotationHorizontal = transform.localEulerAngles.y + rotateY * _speedRotateHorizontal * Time.deltaTime;
            float rotationVertical = transform.localEulerAngles.x + rotateX * _speedRotateVertical * Time.deltaTime;
            transform.rotation = Quaternion.Euler(rotationVertical, rotationHorizontal, 0);
        }
    }

    public void Scroll(float delta)
    {
        if (!_isRotateAroundObject) return;

        //_currentZoom = Vector3.Distance(transform.position, _rotateAroundObjectTransform.position);
        _currentDistanceByCenter += -delta * _speedScroll * Time.deltaTime;

        Vector3 direction = (transform.position - _rotateAroundObjectTransform.position).normalized * _currentDistanceByCenter;
        transform.position = _rotateAroundObjectTransform.position + direction;

        ClampDistanceToCenter(_rotateAroundObjectTransform.position, _minZoom, _maxZoom);
        _currentDeltaPosition = transform.position - _rotateAroundObjectTransform.position;

        //Vector3 direction = (transform.position - _rotateAroundObjectTransform.position).normalized;
        //direction = direction * -delta * _speedScroll * Time.deltaTime;
        //transform.position += direction;

        //_currentZoom += -delta * _speedScroll * Time.deltaTime;
        //_currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);

        //_currentDeltaPosition = transform.position - _rotateAroundObjectTransform.position;
        //ClampDistanceToCenter(_rotateAroundObjectTransform.position, _currentZoom);
        //_currentMaxDistanceByCenter = Vector3.Distance(transform.position, _rotateAroundObjectTransform.position);

    }

    private void LateUpdate()
    {
        if (_isRotateAroundObject)
        {
            transform.position = _rotateAroundObjectTransform.position + _currentDeltaPosition;
        }
        else
        {
            ClampDistanceToCenter(Vector3.zero, 0, _maxDistanceByCenter);
        }
        

    }

    public void RotateAroundObject(Transform objectTransform)
    {
        _isRotateAroundObject = !_isRotateAroundObject;
        if(_isRotateAroundObject)
        {
            _rotateAroundObjectTransform = objectTransform;

            transform.LookAt(_rotateAroundObjectTransform.position);
            ClampDistanceToCenter(_rotateAroundObjectTransform.position, _minZoom, _maxZoom);
            _currentDeltaPosition = transform.position - _rotateAroundObjectTransform.position;


            //_currentMaxDistanceByCenter = Vector3.Distance(transform.position, _rotateAroundObjectTransform.position);
        }
        else
        {
            _rotateAroundObjectTransform = null;
        }
          
    }

    private void ClampDistanceToCenter(Vector3 center, float minDistance, float maxDistance)
    {
        float distance = Vector3.Distance(transform.position, center);
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        _currentDistanceByCenter = distance;
        //if (Vector3.Distance(transform.position, center) > maxDistance)
        //{
            Vector3 direction = (transform.position - center).normalized * distance;
            transform.position = center + direction;
        
        //}
    }
}
