using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoPanel : MonoBehaviour
{
    public void OnClickCloseButton()
    {
        gameObject.SetActive(false);
    }

    public virtual void Show(IRaycasterListener listener)
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        ActionCircle.OnInfo -= Show;
    }

    public void Init() => ActionCircle.OnInfo += Show;
}
