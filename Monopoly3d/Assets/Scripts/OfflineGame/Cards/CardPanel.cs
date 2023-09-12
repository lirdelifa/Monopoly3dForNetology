using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Zenject;

public class CardPanel : MonoBehaviour, IPointerClickHandler
{
    [Inject] private MoneyCountingSystem _moneyCountingSystem;
    [SerializeField] private List<DropCardParameters> _dropCardParameters;
    [SerializeField] private List<ChanceCardParameters> _chanceCardParameters;
    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private TMP_Text _aboutText;

    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.SetActive(false);
    }

    public void ShowRandomDropCard(Character character)
    {
        gameObject.SetActive(true);
        int randId = Random.Range(0, _dropCardParameters.Count);
        _labelText.text = "дпно";
        _aboutText.text = _dropCardParameters[randId].aboutText;
        DropCardEffect(_dropCardParameters[randId].cardType, character);
    }

    private void DropCardEffect(DropCardsType cardsType, Character character)
    {
        switch(cardsType)
        {
            case DropCardsType.AddHealth:
                character.AddHealth(20);
                character.Resume();
                break;
            case DropCardsType.AddDamage:
                character.TakeDamage(20, Vector3.zero);
                if(character != null) character.Resume();
                break;
        }
    }

    public void ShowRandomChanceCard(Character character)
    {
        gameObject.SetActive(true);
        int randId = Random.Range(0, _chanceCardParameters.Count);
        _labelText.text = "ьюмя";
        _aboutText.text = _chanceCardParameters[randId].aboutText;
        ChanceCardEffect(_chanceCardParameters[randId].cardType, character);
    }

    private void ChanceCardEffect(ChanceCardsType cardsType, Character character)
    {
        switch (cardsType)
        {
            case ChanceCardsType.Arrest:
                Vector3 point = CellsData.GetFreePointByCell(CellsData.GetPrisonCell);
                Vector3 newPosition = new Vector3(point.x, character.GetPoint.y, point.z);
                character.transform.position = newPosition;
                character.Arrest();
                break;
            case ChanceCardsType.AddMoney:
                _moneyCountingSystem.AddCashForCharacter(character, 200);
                character.Resume();
                break;
        }
    }
}
