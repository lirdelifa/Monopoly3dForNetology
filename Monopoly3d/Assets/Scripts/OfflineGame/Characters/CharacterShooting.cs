using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
    [SerializeField] private float _timeForRotateOnTarget;
    [SerializeField] private int _damage;
    [SerializeField] private Transform _scope;
    [SerializeField] private Animator _animator;

    private const float _rayLenght = 100f;

    public void Shoot(Vector3 target)
    {
        StartCoroutine(ShootCoroutine(target));
    }

    public void EndShoot() => StopAllCoroutines();

    private IEnumerator ShootCoroutine(Vector3 target)
    {
        Quaternion startRotation = transform.rotation;
        transform.LookAt(target);
        Quaternion endRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.rotation = startRotation;

        float timer = 0;
        while (timer < _timeForRotateOnTarget)
        {
            timer += Time.deltaTime;
            float percentageComplete = timer / _timeForRotateOnTarget;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, percentageComplete);
            yield return null;
        }
        transform.rotation = endRotation;
        _animator.SetTrigger("Shoot");
        OnEndShoot?.Invoke();
        Raycast();
    }

    private void Raycast()
    {
        RaycastHit hit;
        if(Physics.Raycast(_scope.transform.position, transform.forward, out hit, _rayLenght))
        {
            IDamagable damagable = (IDamagable)hit.collider.GetComponent(typeof(IDamagable));
            if (damagable == null) return;
            damagable.TakeDamage(_damage, _scope.transform.position);
        }
    }

    public delegate void EndShootDelegate();
    public event EndShootDelegate OnEndShoot;
}
