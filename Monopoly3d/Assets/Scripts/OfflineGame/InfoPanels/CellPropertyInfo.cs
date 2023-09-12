using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CellPropertyInfo : InfoPanel
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _ownerText;
    [SerializeField] private TMP_Text _buyPriceText;
    [SerializeField] private TMP_Text _protectionWallPriceText;
    [SerializeField] private TMP_Text _rentText;
    [SerializeField] private TMP_Text _protectionWallAddedRentText;
    [SerializeField] private TMP_Text _fullGroupAddedRentText;
    [SerializeField] private TMP_Text _allRentText;
    [SerializeField] private TMP_Text _sellPriceText;
    [SerializeField] private GameObject _allRentLabel;
    [SerializeField] private GameObject _sellPriceLabel;

    public override void Show(IRaycasterListener listener)
    {
        if (!(listener is TargetPoint)) return;

        base.Show(listener);

        CellProperty cellProperty = CellsData.GetCellPropertyById(CellsData.DefiningCellIdByPosition(listener.GetPoint));

        _nameText.text = cellProperty.GetName;
        _buyPriceText.text = cellProperty.GetMoneyParameters.buyPrice.ToString() + "$";
        _protectionWallPriceText.text = cellProperty.GetMoneyParameters.protectionWallPrice.ToString() + "$";
        _rentText.text = cellProperty.GetMoneyParameters.rent.ToString() + "$";
        _protectionWallAddedRentText.text = cellProperty.GetMoneyParameters.protectionWallAddedRent.ToString() + "$";
        _fullGroupAddedRentText.text = cellProperty.GetMoneyParameters.fullGroupAddedRent.ToString() + "$";

        bool haveOwner = cellProperty.Owner != null;

        _allRentLabel.SetActive(haveOwner);
        _sellPriceLabel.SetActive(haveOwner);
        _allRentText.gameObject.SetActive(haveOwner);
        _sellPriceText.gameObject.SetActive(haveOwner);

        if (haveOwner)
        {
            _ownerText.text = cellProperty.Owner.Name;
            _allRentText.text = cellProperty.CalculateRent().ToString() + "$";
            _sellPriceText.text = cellProperty.CalculateSellPrice().ToString() + "$";
        }
        else _ownerText.text = "-----";
    }
}
