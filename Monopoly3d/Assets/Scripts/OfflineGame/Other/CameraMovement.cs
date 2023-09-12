using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    [SerializeField, Header("—корость перемещени€")] private float _speedMovement;
    [SerializeField, Header("¬ертикальна€/горизонтальна€ скорость вращени€")] private Vector2 _speedRotation;
    [SerializeField, Header("¬ерхний/нижний лимит поворота по вертикали (0-85)")] private Vector2 _limitingVerticalRotationAngles;

    private const int _straightAngle = 90;
    private float _sensitivityMultiplier;

    private void Start()
    {
        if (PlayerPrefs.HasKey(SettingsPanel.MouseSensitivityPPName)) _sensitivityMultiplier = PlayerPrefs.GetFloat(SettingsPanel.MouseSensitivityPPName);
        else _sensitivityMultiplier = SettingsPanel.DefaultMouseSensitivity;
    }


    public void Move(float forward, float right)
    {
        Vector3 direction = Vector3.Normalize(transform.forward * forward + transform.right * right);
        direction = direction * _speedMovement * Time.deltaTime;
        transform.position += direction;
    }

    public void Rotate(float vertical, float horizontal)
    {
        if (vertical > 0 && _straightAngle - Vector3.Angle(transform.forward, Vector3.up) > _limitingVerticalRotationAngles.y) return;
        if (vertical < 0 && _straightAngle - Vector3.Angle(transform.forward, -Vector3.up) > _limitingVerticalRotationAngles.x) return;

        float rotationVertical = transform.localEulerAngles.x - vertical * _speedRotation.x * Time.deltaTime * _sensitivityMultiplier;
        float rotationHorizontal = transform.localEulerAngles.y + horizontal * _speedRotation.y * Time.deltaTime * _sensitivityMultiplier;
        transform.rotation = Quaternion.Euler(rotationVertical, rotationHorizontal, 0);
    }

    private void ChangeVale(float newValue)
    {
        _sensitivityMultiplier = newValue;
    }

    private void OnEnable()
    {
        SettingsPanel.OnChangeMouseSensitivity += ChangeVale;
    }

    private void OnDisable()
    {
        SettingsPanel.OnChangeMouseSensitivity -= ChangeVale;
    }
}
