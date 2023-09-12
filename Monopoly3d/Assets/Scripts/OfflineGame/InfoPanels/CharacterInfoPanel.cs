using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterInfoPanel : InfoPanel
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _controlText;
    [SerializeField] private TMP_Text _cashText;
    [SerializeField] private TMP_Text _monetaryConditionText;

    public override void Show(IRaycasterListener listener)
    {
        if (!(listener is Character)) return;

        base.Show(listener);
        Character character = (Character)listener;
        _nameText.text = character.Name;
        _controlText.text = character.IsBotControlled ? "¡Œ“" : "¬€";
        _cashText.text = character.Cash.ToString() + "$";
        _monetaryConditionText.text = character.MonetaryCondition.ToString() + "$";
    }
}
