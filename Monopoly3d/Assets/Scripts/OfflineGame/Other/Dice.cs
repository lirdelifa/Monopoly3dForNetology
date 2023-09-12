using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : SelectableObject
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Vector2 _limitingHeights;
    [SerializeField] private Vector2 _limitingAngularSpeeds;
    [SerializeField] private Vector3 _startPosition;

    public bool IsEndFlip { get; private set; }
    public int FellOutNumber { get; private set; }

    public void Flip() => StartCoroutine(FlipCoroutine());
    
    private IEnumerator FlipCoroutine()
    {
        IsEndFlip = false;

        transform.position = new Vector3(_startPosition.x, Random.Range(_limitingHeights.x, _limitingHeights.y), _startPosition.z);
        transform.rotation = Random.rotation;
        _rigidbody.angularVelocity = new Vector3(Random.Range(_limitingAngularSpeeds.x, _limitingAngularSpeeds.y), Random.Range(_limitingAngularSpeeds.x, _limitingAngularSpeeds.y), Random.Range(_limitingAngularSpeeds.x, _limitingAngularSpeeds.y));

        while (_rigidbody.angularVelocity != Vector3.zero || _rigidbody.velocity != Vector3.zero)
        {
            yield return null;
        }

        FellOutNumber = CheckFace();
        IsEndFlip = true;
        OnEndFlip?.Invoke();
    }

    private int CheckFace()
    {
        if (CheckRaycast(transform.forward)) return 1;
        if (CheckRaycast(-transform.forward)) return 6;
        if (CheckRaycast(transform.right)) return 3;
        if (CheckRaycast(-transform.right)) return 5;
        if (CheckRaycast(transform.up)) return 4;
        return 2;
    }

    private bool CheckRaycast(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 1f))
        {
            if (hit.collider.GetComponent<PlaneCollider>()) return true;
        }
        return false;
    }

    public delegate void EndFlip();
    public static event EndFlip OnEndFlip;

}
