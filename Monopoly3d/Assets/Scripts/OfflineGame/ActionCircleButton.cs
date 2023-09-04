using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionCircleButton : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _iconCircle;
    [SerializeField] private ActionCircle _actionCircle;
    private const float AlphaHitTestMinimumThreshold = 1f;
    public Color LockedColor { get; set; }
    public Color SelectedColor { get; set; }
    public Color DeselectedColor { get; set; }
    public float Delta { get; set; }
    public float MinRaycastPadding { get; set; }
    public float MaxRaycastPadding { get; set; }
    public ActionButtonType ButtonType { get; set; }
    

    public bool IsLocked { get; set; }

    private bool _isSelected = false;
    private GameObject _currentIcon;



    private void Start()
    {
        _image.alphaHitTestMinimumThreshold = AlphaHitTestMinimumThreshold;
    }

    //[SerializeField] private Animator _animator;
    private void OnDisable()
    {
        if(_isSelected) transform.localPosition -= new Vector3(0, Delta, 0);
        if(_currentIcon != null) Destroy(_currentIcon);
        _iconCircle.SetActive(false);

    }

    public void SetIcon(GameObject icon)
    {
        _iconCircle.SetActive(true);
        _currentIcon = Instantiate(icon, _iconCircle.transform);
        _currentIcon.transform.localPosition = Vector3.zero;
    }

    public void Show()
    {
        _isSelected = false;
        if(IsLocked)
        {
            _image.color = LockedColor;
        }
        else
        {
            _image.color = DeselectedColor;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void OnSelected()
    {
        if (IsLocked) return;
        _image.color = SelectedColor;
        transform.localPosition += new Vector3(0, Delta, 0);
        _isSelected = true;
        _actionCircle.ChangeButton(this, true);
        
    }

    public void OnDeSelected()
    {
        if (IsLocked) return;
        _image.color = DeselectedColor;
        transform.localPosition -= new Vector3(0, Delta, 0);
        _isSelected = false;
        _actionCircle.ChangeButton(this, false);
    }


}
