using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CharacterInteractionWithGUI : MonoBehaviour
{
    [SerializeField] private Transform _canvasTransform;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Color _colorWthenControlledByPlayer;
    [SerializeField] private Color _colorWthenControlledByBot;
    [SerializeField] private Image _backgroundNameImage;
    [SerializeField] private Image _healthImage;
    [SerializeField] private GameObject _addMoneyMessagePrefab;
    [SerializeField] private RectTransform _addMoneyMessagesTransform;
    [SerializeField] private float _timeBetweenMessages;

    private List<AddMoneyMessage> _addMoneyMessagesPool = new List<AddMoneyMessage>();
    private Dictionary<AddMoneyMessage, int> _holdMoneyMessages = new Dictionary<AddMoneyMessage, int>();

    private float _timerBetweenMessages = 0;

    private Transform _cameraTransform;
    private const float _maxHealth = 100f;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    public void SetNameText(string name) => _nameText.text = name;

    public void ChangeColor(bool isControlledByPlayer) => _backgroundNameImage.color = isControlledByPlayer ? _colorWthenControlledByPlayer : _colorWthenControlledByBot;

    public void ChangeHealth(int newHealth)
    {
        _healthImage.fillAmount = (float)newHealth / _maxHealth;
    }

    private void Update()
    {
        _canvasTransform.LookAt(_cameraTransform.position);
        Quaternion newRotation = Quaternion.Euler(0f, _canvasTransform.rotation.eulerAngles.y, 0f);
        _canvasTransform.rotation = newRotation;

        if (_holdMoneyMessages.Count > 0)
        {
            if (_timerBetweenMessages > 0)
            {
                _timerBetweenMessages -= Time.deltaTime;
            }
            else
            {
                var first = _holdMoneyMessages.First();
                AddMoneyMessage addMoneyMessage = first.Key;
                addMoneyMessage.gameObject.SetActive(true);
                addMoneyMessage.transform.localPosition = Vector3.zero;
                addMoneyMessage.Show(first.Value);
                _holdMoneyMessages.Remove(first.Key);
                addMoneyMessage.OnEndLife += EndLifeMoneyMessage;
                _timerBetweenMessages = _timeBetweenMessages;
                if (_holdMoneyMessages.Count == 0) _timerBetweenMessages = 0;
            }
        }
    }

    private void EndLifeMoneyMessage(AddMoneyMessage addMoneyMessage)
    {
        _addMoneyMessagesPool.Add(addMoneyMessage);
        addMoneyMessage.gameObject.SetActive(false);
        addMoneyMessage.OnEndLife -= EndLifeMoneyMessage;
    }

    public void AddMoney(int value)
    {
        AddMoneyMessage addMoneyMessage;
        if (_addMoneyMessagesPool.Count > 0)
        {
            addMoneyMessage = _addMoneyMessagesPool[0];
            _addMoneyMessagesPool.Remove(addMoneyMessage);
        }
        else
        {
            addMoneyMessage = Instantiate(_addMoneyMessagePrefab, _addMoneyMessagesTransform).GetComponent<AddMoneyMessage>();
        }
        addMoneyMessage.gameObject.SetActive(false);
        _holdMoneyMessages.Add(addMoneyMessage, value);
    }
}
