using System.Collections;
using UnityEngine;

public class DiceMenu : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float _rayLength;
    [SerializeField] private Vector3 _endLoadPosition;
    [SerializeField] private Vector3 _endLoadRotation;
    [SerializeField] private Vector3 _endLoadScale;
    [SerializeField, Range(0f, 10f)] private float _timeToLoadTransform;
    private Camera _camera;
    private bool _isLookToMouse = true;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!_isLookToMouse) return;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = ray.GetPoint(_rayLength);
        transform.LookAt(targetPoint);
    }

    public void StartGoToLoadAnimation()
    {
        _isLookToMouse = false;
        StartCoroutine(GoToLoadAnimation());
    }

    private IEnumerator GoToLoadAnimation()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 startScale = transform.localScale;
        Quaternion startRotation = transform.rotation;
        Quaternion endLoadRotation = Quaternion.Euler(_endLoadRotation);

        while(elapsedTime < _timeToLoadTransform)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / _timeToLoadTransform;
            transform.position = Vector3.Lerp(startPosition, _endLoadPosition, percentageComplete);
            transform.rotation = Quaternion.Lerp(startRotation, endLoadRotation, percentageComplete);
            transform.localScale = Vector3.Lerp(startScale, _endLoadScale, percentageComplete); 

            yield return null;
        }
        OnEndDiceAnimation?.Invoke();
    }

    public delegate void EndDiceAnimation();
    public event EndDiceAnimation OnEndDiceAnimation;
}
