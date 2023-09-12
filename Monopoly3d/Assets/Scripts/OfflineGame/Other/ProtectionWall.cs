using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ProtectionWall : SelectableObject
{
    [SerializeField] private Vector3 _showScale;
    [SerializeField] private Vector3 _hideScale;
    [SerializeField] private Vector3 _showPosition;
    [SerializeField] private Vector3 _hidePosition;
    [SerializeField] private float _timeForAnimate;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private CellProperty _cell;

    public int Health { get; private set; }

    public void SetHealth(int value)
    {
        Health = value;
        _healthText.text = Health.ToString() + "%"; 
    }   

    public int MaxHealth { get; private set; } = 100;
    public bool IsShow { get; private set; } = false;
    public CellProperty GetCell { get => _cell; }

    public void Show(bool isShow)
    {
        IsShow = isShow;
        StartCoroutine(MoveCoroutine(isShow));
        Health = MaxHealth;
        _healthText.text = Health.ToString() + "%";
    }

    

    private IEnumerator MoveCoroutine(bool isShow)
    {
        transform.localPosition = isShow ? _showPosition : _hidePosition;
        Vector3 startPosition = transform.localPosition;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = isShow ? _showScale : _hideScale;
        Vector3 endPosition = isShow ? _showPosition : _hidePosition;
        float timer = 0;
        while(timer < _timeForAnimate)
        {
            timer += Time.deltaTime;
            float percentageComplete = timer / _timeForAnimate;
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, percentageComplete);
            transform.localScale = Vector3.Lerp(startScale, endScale, percentageComplete);
            yield return null;
        }
        transform.localPosition = endPosition;
        transform.localScale = endScale;
        _healthText.gameObject.SetActive(IsShow);
    }

    public override void TakeDamage(int damage, Vector3 direction)
    {
        base.TakeDamage(damage, direction);
        Health -= damage;
        if(Health <= 0)
        {
            Show(false);
        }
        else _healthText.text = Health.ToString() + "%";
        OnHit?.Invoke();
    }

    public ProtectionWallSaveParameters GetSaveParameters()
    {
        ProtectionWallSaveParameters parameters = new ProtectionWallSaveParameters();
        for(int i = 0; i < _cell.GetProtectionWalls.Count; i++)
        {
            if (_cell.GetProtectionWalls[i] == this)
            {
                parameters.id = i;
                break;
            }
        }

        parameters.cellId = _cell.GetId;
        parameters.health = Health;
        parameters.isShow = IsShow;
        
        return parameters;
    }

    public delegate void Hit();
    public static event Hit OnHit;
}
