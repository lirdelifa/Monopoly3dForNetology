using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIHandler : MonoBehaviour
{
    [SerializeField] private ActionCircle _actionCircle;
    [SerializeField] private Image _loadActionCircleImage;
    [SerializeField, Range(0f, 5f)] float _timeToShowActionCircle;
    private Coroutine _timerBeforeShowActionCircle;

    private void Start()
    {
        _loadActionCircleImage.gameObject.SetActive(false);
        _actionCircle.Hide();
    }

    public void ShowActionCircle(PointerHandler pointerHandler)
    {
        if(_timerBeforeShowActionCircle != null)
        {
            HideActionCircle();
        }
        _timerBeforeShowActionCircle = StartCoroutine(TimerBeforeShowActionCircle(pointerHandler));
    }

    public void HideActionCircle()
    {
        StopCoroutine(_timerBeforeShowActionCircle);
        _loadActionCircleImage.gameObject.SetActive(false);
        _timerBeforeShowActionCircle = null;
        _actionCircle.Hide();
    }

    private IEnumerator TimerBeforeShowActionCircle(PointerHandler pointerHandler)
    {
        _loadActionCircleImage.gameObject.SetActive(true);
        ArrangementElementsDependingOnResolution(_loadActionCircleImage.GetComponent<RectTransform>());
        float timer = 0;
        while(timer < _timeToShowActionCircle) 
        {
            timer += Time.deltaTime;
            _loadActionCircleImage.fillAmount = timer / _timeToShowActionCircle;
            yield return null;
        }
        _loadActionCircleImage.gameObject.SetActive(false);
        _actionCircle.Show(pointerHandler);
        ArrangementElementsDependingOnResolution(_actionCircle.GetComponent<RectTransform>());
    }

    private void ArrangementElementsDependingOnResolution(RectTransform rectTransform)
    {
        float mouseX = Input.mousePosition.x - Screen.width / 2;
        float mouseY = Input.mousePosition.y - Screen.height / 2;

        float maxX = Screen.width / 2 - rectTransform.sizeDelta.x / 2;
        float maxY = Screen.height / 2 - rectTransform.sizeDelta.y / 2;
        mouseX = Mathf.Clamp(mouseX, -maxX, maxX);
        mouseY = Mathf.Clamp(mouseY, -maxY, maxY);

        rectTransform.anchoredPosition = new Vector3(mouseX, mouseY, 0f);
    }
    
}
