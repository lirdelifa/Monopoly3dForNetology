using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using static UnityEngine.GraphicsBuffer;

public class UnitMove : MonoBehaviour
{
    [Inject] private CellsData _cellsData;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed;
    private const float _distanceToCompareWithTarget = 0.1f;

    private int CurrentCellId { get; set; }

    private void Start()
    {
        //Потом в GameStart прокидывать
        CurrentCellId = _cellsData.GetStartCellId;
    }

    public void MovingTo(Vector3 targetPosition, int targetCellId)
    {
        StartCoroutine(Moving(targetPosition, targetCellId));
    }

    private void RotateAt(Vector3 target)
    {
        transform.LookAt(target);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    private IEnumerator Moving(Vector3 targetPosition, int targetCellId) 
    {
        Vector3 currentTarget;
        bool isEndMoving = false;
        while (!isEndMoving) 
        {
            if (transform.position == targetPosition)
            {
                _animator.SetBool("Run", false);
                isEndMoving = true;
                continue;
            }

            currentTarget = _cellsData.GetCornerCellOnRay(CurrentCellId, targetCellId).transform.position;
            CurrentCellId = _cellsData.GetCornerCellOnRay(CurrentCellId, targetCellId).Id;
            print(CurrentCellId);

            if (CurrentCellId == targetCellId) currentTarget = targetPosition;

            RotateAt(currentTarget);
            _animator.SetBool("Run", true);

            while (Vector3.Distance(transform.position, currentTarget) > _distanceToCompareWithTarget)
            {
                transform.position += transform.forward * _speed * Time.deltaTime;
                yield return null;
            }

            //_animator.SetBool("Run", false);
            transform.position = currentTarget;
            

            yield return null;
        }
    }
}
