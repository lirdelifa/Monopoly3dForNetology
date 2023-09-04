using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StandartCell : Cell
{
    [SerializeField] private CellParameters _cellParameters;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _groupText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Image _border;

    public CellGroupType GetGroupType { get => _cellParameters.groupType; }
    public void SetMaterial(Material material) => _border.material = material;
    public void SetGroupName(string name) => _groupText.text = name;

    private void Start()
    {
        _nameText.text = _cellParameters.name;
        _priceText.text = _cellParameters.price.ToString() + "$";
    }
}
