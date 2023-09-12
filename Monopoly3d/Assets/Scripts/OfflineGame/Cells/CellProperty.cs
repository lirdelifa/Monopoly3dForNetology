using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CellProperty : Cell
{
    [SerializeField] private TMP_Text _ownerNameText;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private GameObject _ownerInfo;
    [SerializeField] private CellPropertyMoneyParameters _moneyParameters;
    [SerializeField] private string _name;
    [SerializeField] private List<ProtectionWall> _protectionWalls;
    [SerializeField] private Image _borderImage;
    [SerializeField] private CellGroupType _groupType;

    private const float _multiplierSellPrice = 0.75f;

    public string GetName { get => _name; }

    private void Start()
    {
        _nameText.text = _name;
        _priceText.text = _moneyParameters.buyPrice.ToString() + "$";
    }

    public List<ProtectionWall> GetProtectionWalls { get => _protectionWalls; }

    public CellPropertyMoneyParameters GetMoneyParameters { get => _moneyParameters; }

    public CellGroupType GetGroupType { get => _groupType; }

    public int CalculateRent()
    {
        int rent = _moneyParameters.rent;
        for(int i = 0; i < _protectionWalls.Count; i++) 
        {
            if (_protectionWalls[i].IsShow) rent += _moneyParameters.protectionWallAddedRent;
        }
        if (CellsData.CheckFullGroup(_groupType)) rent += _moneyParameters.fullGroupAddedRent;
        return rent;
    }

    public int CalculateSellPrice()
    {
        int price = _moneyParameters.buyPrice;
        for (int i = 0; i < _protectionWalls.Count; i++)
        {
            if (_protectionWalls[i].IsShow) price += (int)((float)_moneyParameters.protectionWallPrice * ((float)_protectionWalls[i].Health / (float)_protectionWalls[i].MaxHealth));
        }
        return (int)((float)price * _multiplierSellPrice);
    }

    public int GetSellProtectionWallPrice { get => (int)((float)_moneyParameters.protectionWallPrice * _multiplierSellPrice); }

    public void SetBorderColor(Color color) => _borderImage.color = color;

    public Character Owner { get; private set; }

    public PropertyCellSaveParameters GetSaveParameters()
    {
        PropertyCellSaveParameters parameters = new PropertyCellSaveParameters();
        parameters.id = _id;
        parameters.haveOwner = Owner != null;
        if(Owner != null)
        {
            for (int i = 0; i < CharactersData.GetCharacters.Count; i++)
            {
                if (CharactersData.GetCharacters[i] == Owner)
                {
                    parameters.ownerId = i;
                    break;
                }
            }
        }

        return parameters;
    }

    public void SetOwner(Character character)
    {
        Owner = character;

        if (character == null)
        {
            _ownerInfo.SetActive(false);
            for (int i = 0; i < _protectionWalls.Count; i++) _protectionWalls[i].Show(false);
            return;
        }

        _ownerNameText.text = character.Name;
        _ownerInfo.SetActive(true);
    }
}
