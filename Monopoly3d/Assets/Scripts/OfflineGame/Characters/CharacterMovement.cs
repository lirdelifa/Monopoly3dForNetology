using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField, Header("Скорость перемещения")] private float _speedMovement;
    private const float _maxDistanceToStopMoving = 0.1f;

    private bool _isMoving = false;
    public bool IsMoving { get => _isMoving; }

    public void Move(List<Vector3> points)
    {
        if (_isMoving) return;
        StartCoroutine(Movement(points));
    }

    private IEnumerator Movement(List<Vector3> points)
    {
        _isMoving = true;

        for (int i = 0; i < points.Count; i++) 
        {
            Vector3 position = new Vector3(points[i].x, transform.position.y, points[i].z);
            transform.LookAt(position);
            Quaternion newRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            transform.rotation = newRotation;
            while(Vector3.Distance(transform.position, position) > _maxDistanceToStopMoving)
            {
                Vector3 direction = transform.forward * _speedMovement * Time.deltaTime;
                transform.position += direction;
                yield return null;
            }
            transform.position = position;
            yield return points[i];
        }

        OnEndMove?.Invoke();
        _isMoving = false;
    }

    public delegate void EndMoveDelegate();
    public event EndMoveDelegate OnEndMove;
}
