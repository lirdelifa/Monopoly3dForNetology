using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SaveLine : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Button _deleteButton;
    [SerializeField] private Image _backGroundImage;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _deselectedColor;

    private bool _selected;
    private LoadGamePanel _loadGamePanel;
    public string GetName { get => _nameText.text; }

    private void Start()
    {
        _deleteButton.onClick.AddListener(Delete);
    }

    public void SetName(string name) => _nameText.text = name;
    public void SetLoadGamePanel(LoadGamePanel loadGamePanel) => _loadGamePanel = loadGamePanel;


    public void OnPointerClick(PointerEventData eventData)
    {
        _selected = !_selected;
        _loadGamePanel.SelectSaveLine(_selected ? this : null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Select(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_selected) return;
        Select(false);
    }

    public void Select(bool isSelect) => _backGroundImage.color = isSelect ? _selectedColor : _deselectedColor;
    public void Deselect()
    {
        _selected = false;
        Select(false);
    }

    private void Delete()
    {
        _loadGamePanel.DeleteSave(this);
        Destroy(gameObject);
    }
}
