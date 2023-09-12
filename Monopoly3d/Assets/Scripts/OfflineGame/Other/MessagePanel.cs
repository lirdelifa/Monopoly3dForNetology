using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MessagePanel : MonoBehaviour
{
    [SerializeField] private RectTransform _arrowButton;
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _messagePrefab;
    [SerializeField] private Transform _contentTransform;
    [SerializeField] private Transform _trashBasked;

    private List<Message> _messages = new List<Message>();

    private void Start()
    {
        for(int i = 0; i < _maxCountMessages; i++) 
        {
            Message newMessage = Instantiate(_messagePrefab, _trashBasked).GetComponent<Message>();
            newMessage.gameObject.SetActive(false);
            _messages.Add(newMessage);
        }
    }

    private const int _maxCountMessages = 20;
    private const float _rotateArrowButtonAngle = 180f;
    private bool _enabledPanel = false;

    public void OnClickArrowButton()
    {
        _enabledPanel = !_enabledPanel;
        _mainPanel.SetActive(_enabledPanel);
        Quaternion newRotation = Quaternion.Euler(0, 0, _arrowButton.localEulerAngles.z + _rotateArrowButtonAngle);
        _arrowButton.localRotation = newRotation;
    }

    public void NewMessage(string text)
    {
        Message newMessage = _messages[0];
        newMessage.gameObject.SetActive(true);
        _messages.RemoveAt(0);
        newMessage.transform.SetParent(_trashBasked);
        newMessage.transform.SetParent(_contentTransform);
        Vector3 newPosition = new Vector3(newMessage.transform.localPosition.x, newMessage.transform.localPosition.y, 0);
        newMessage.transform.localPosition = newPosition;
        _messages.Add(newMessage);
        newMessage.SetText(text);
    }
}
