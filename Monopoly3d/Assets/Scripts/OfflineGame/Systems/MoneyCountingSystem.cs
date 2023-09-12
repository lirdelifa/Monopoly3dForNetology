using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoneyCountingSystem : IDispose
{
    private const int _moneyForStartCell = 200;

    MoneyCountingSystem()
    {
        ProtectionWall.OnHit += CalculateAllMonetotyCondition;
    }

    public void CalculateAllMonetotyCondition()
    {
        for (int i = 0; i < CharactersData.GetCharacters.Count; i++)
        {
            CharactersData.GetCharacters[i].SetMonetaryCondition(CalculateMonetaryCondition(CharactersData.GetCharacters[i]));
        }
    }

    public void AddMoneyForStartCell(Character character)
    {
        character.AddCash(_moneyForStartCell);
        character.SetMonetaryCondition(CalculateMonetaryCondition(character));
    }

    public void AddCashForCharacter(Character character, int money)
    {
        character.AddCash(money);
        character.SetMonetaryCondition(CalculateMonetaryCondition(character));
    }

    public int TryPay(Character character)
    {
        CellProperty cellProperty = CellsData.GetCellPropertyById(CellsData.DefiningCellIdByPosition(character.GetPoint));
        if (cellProperty.CalculateRent() <= character.Cash)
        {
            character.AddCash(-cellProperty.CalculateRent());
            character.SetMonetaryCondition(CalculateMonetaryCondition(character));
            cellProperty.Owner.AddCash(cellProperty.CalculateRent());
            cellProperty.Owner.SetMonetaryCondition(CalculateMonetaryCondition(cellProperty.Owner));
            return 0;
        }
        else if (cellProperty.CalculateRent() <= character.MonetaryCondition)
        {
            int needMoney = cellProperty.CalculateRent() - character.Cash;
            cellProperty.Owner.AddCash(character.Cash);
            character.AddCash(-character.Cash);
            cellProperty.Owner.SetMonetaryCondition(CalculateMonetaryCondition(cellProperty.Owner));
            character.SetMonetaryCondition(CalculateMonetaryCondition(character));
            return needMoney;
        }
        else return -1;
    }

    public void SellOnNeedMoney(Character character, int needMoney)
    {
        int moneyFromSale = 0;
        List<CellProperty> cellProperties = CellsData.GetCellProperties();
        for (int i = 0; i < cellProperties.Count; i++)
        {
            if (cellProperties[i].Owner != character) continue;
            List<ProtectionWall> protectionWalls = cellProperties[i].GetProtectionWalls.Where(s => s.IsShow == true).ToList();
            for (int b = 0; b < protectionWalls.Count; b++)
            {
                protectionWalls[b].Show(false);
                moneyFromSale += cellProperties[i].GetSellProtectionWallPrice;
                if (moneyFromSale >= needMoney)
                {
                    character.AddCash(moneyFromSale);
                    character.SetMonetaryCondition(CalculateMonetaryCondition(character));
                    return;
                }
            }
        }
        for (int i = 0; i < cellProperties.Count; i++)
        {
            if (cellProperties[i].Owner != character) continue;
            cellProperties[i].SetOwner(null);
            moneyFromSale += cellProperties[i].CalculateSellPrice();
            if (moneyFromSale >= needMoney)
            {
                character.AddCash(moneyFromSale);
                character.SetMonetaryCondition(CalculateMonetaryCondition(character));
                return;
            }
        }
    }
    public void SellCell(Character character, CellProperty cellProperty)
    {
        character.AddCash(cellProperty.CalculateSellPrice());
        cellProperty.SetOwner(null);
        character.SetMonetaryCondition(CalculateMonetaryCondition(character));
    }

    public void SellUpgrade(Character character, ProtectionWall protectionWall)
    {
        character.AddCash(protectionWall.GetCell.GetSellProtectionWallPrice);
        protectionWall.Show(false);
        character.SetMonetaryCondition(CalculateMonetaryCondition(character));
    }

    public void TryBuyCell(Character character)
    {
        CellProperty cellProperty = CellsData.GetCellPropertyById(CellsData.DefiningCellIdByPosition(character.GetPoint));
        if (cellProperty == null) return;
        if (cellProperty.Owner != null) return;
        if (cellProperty.GetMoneyParameters.buyPrice > character.Cash) return;

        character.AddCash(-cellProperty.GetMoneyParameters.buyPrice);
        cellProperty.SetOwner(character);
        character.SetMonetaryCondition(CalculateMonetaryCondition(character));
    }

    public void TryBuyUpgrade(Character character, ProtectionWall protectionWall)
    {
        if (character.Cash >= protectionWall.GetCell.GetMoneyParameters.protectionWallPrice)
        {
            character.AddCash(-protectionWall.GetCell.GetMoneyParameters.protectionWallPrice);
            protectionWall.Show(true);
            character.SetMonetaryCondition(CalculateMonetaryCondition(character));
            return;
        }
    }

    public void TryBuyRandUpgrade(Character character)
    {
        List<CellProperty> cellProperties = CellsData.GetCellProperties();

        for (int i = 0; i < cellProperties.Count; i++)
        {
            if (cellProperties[i].Owner != character) continue;
            if (cellProperties[i].GetMoneyParameters.protectionWallPrice > character.Cash) return;
            for (int b = 0; b < cellProperties[i].GetProtectionWalls.Count; b++)
            {
                if (!cellProperties[i].GetProtectionWalls[b].IsShow && character.Cash >= cellProperties[i].GetMoneyParameters.protectionWallPrice)
                {
                    character.AddCash(-cellProperties[i].GetMoneyParameters.protectionWallPrice);
                    cellProperties[i].GetProtectionWalls[b].Show(true);
                    character.SetMonetaryCondition(CalculateMonetaryCondition(character));
                    return;
                }
            }
        }
    }

    private int CalculateMonetaryCondition(Character character)
    {
        int monetaryCondition = character.Cash;
        List<CellProperty> cellProperties = CellsData.GetCellProperties();
        for (int i = 0; i < cellProperties.Count; i++)
        {
            if (cellProperties[i].Owner != character) continue;
            monetaryCondition += cellProperties[i].CalculateSellPrice();
        }

        return monetaryCondition;
    }

    public void SellOfEntireState(Character character)
    {
        List<CellProperty> cellProperties = CellsData.GetCellProperties().Where(s => s.Owner == character).ToList();
        for (int i = 0; i < cellProperties.Count; i++)
        {
            cellProperties[i].SetOwner(null);
        }
    }

    public void Dispose()
    {
        ProtectionWall.OnHit -= CalculateAllMonetotyCondition;
    }
}
