using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceLoad : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float _speedRotation;

    private void Update()
    {
        transform.Rotate(_speedRotation * Time.deltaTime, _speedRotation * Time.deltaTime, 0);
    }
}
