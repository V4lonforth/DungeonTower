﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootGenerator : MonoBehaviour
{
    public List<Item> armorItems;
    public List<Item> weaponItems;
    public List<Item> potionItems;
    public List<Item> goldItems;
    public List<Item> chestItems;

    public int GenerateItem(Cell cell, bool chestAllowed = true)
    {
        float value = Random.Range(0f, 1f);
        if (value < 0.15f)
            return GenerateGold(cell);
        else if (value < 0.18f)
            return GenerateItem(GetRandomItem(weaponItems), cell);
        else if (value < 0.21f)
            return GenerateItem(GetRandomItem(potionItems), cell);
        else if (value < 0.24f)
            return GenerateItem(GetRandomItem(armorItems), cell);
        else if (chestAllowed && value < 0.30f)
            return GenerateItem(GetRandomItem(chestItems), cell);
        return 0;
    }

    public void GenerateLoot(Tower tower)
    {
        foreach (Room room in tower.Rooms.Skip(1))
        {
            int roomValue = CalculateRoomValue(room, room.Strength);
            List<Cell> emptyCells = new List<Cell>(room.Cells);
            foreach (Cell cell in emptyCells)
            {
                roomValue -= GenerateItem(cell);
                if (roomValue < 0)
                    break;
            }
        }
    }

    public int GenerateItem(Item item, Cell cell)
    {
        return Item.Instantiate(item, cell).value;
    }

    public int GenerateGold(Cell cell)
    {
        int amount = Random.Range(8, 14);
        foreach (Item item in cell.Items)
            if (item is GoldItem goldItem)
            {
                goldItem.Amount += amount;
                return goldItem.value;
            }
        Item gold = Item.Instantiate(GetRandomItem(goldItems), cell);
        ((GoldItem)gold).Amount = amount;
        return gold.value;
    }

    private T GetRandomItem<T>(List<T> items)
    {
        return items[Random.Range(0, items.Count)];
    }

    private int CalculateRoomValue(Room room, int strength)
    {
        return strength + Random.Range(5, 15) * room.Cells.Count;
    }
}