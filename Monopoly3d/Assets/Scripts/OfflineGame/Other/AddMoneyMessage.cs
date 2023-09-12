using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddMoneyMessage : MonoBehaviour
{
    [SerializeField] private Color _addMoneyColor;
    [SerializeField] private Color _removeMoneyColor;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;
    private float _timer;


    public string GetText { get => _text.text; } 
    public void Show(int value)
    {
        _timer = _lifeTime;
        _text.text = value.ToString() + "$";
        _text.color = value > 0 ? _addMoneyColor : _removeMoneyColor;
    }

    private void Update()
    {
        if (_timer <= 0) return;
        _timer -= Time.deltaTime;
        transform.localPosition += new Vector3(0, _speed * Time.deltaTime, 0);
        if (_timer <= 0) OnEndLife?.Invoke(this);
    }

    public delegate void EndLife(AddMoneyMessage addMoneyMessage);
    public event EndLife OnEndLife;
}
