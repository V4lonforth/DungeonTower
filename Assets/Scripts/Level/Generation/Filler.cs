﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Filler : MonoBehaviour
{
    public Text goldText;

    public List<GameObject> armorItems;
    public List<GameObject> potionItems;
    public List<GameObject> weaponItems;
    public List<GameObject> goldItems;
    public List<GameObject> chestItems;

    public EnemyGroups enemyGroups;

    public GameObject playerPrefab;

    public void Fill(Tower tower)
    {
        foreach (Room room in tower.Rooms.Skip(1))
        {
            int roomStrength = GenerateMonsters(new List<Cell>(room.Cells), CalculateRoomStrength(room));
            int roomValue = CalculateRoomValue(room, roomStrength);
            List<Cell> emptyCells = new List<Cell>(room.Cells);
            foreach (Cell cell in emptyCells)
            {
                roomValue -= GenerateItem(cell);
                if (roomValue < 0)
                    break;
            }
        }
        tower.Player = GeneratePlayer(tower[0, 0]);
    }

    private int GenerateMonsters(List<Cell> emptyCells, int roomStrength)
    {
        EnemyGroup enemyGroup = enemyGroups.GetRandomEnemyGroup(emptyCells.Count, roomStrength);
        int currentStrength = 0;
        while (emptyCells.Count > 0 && currentStrength < roomStrength)
        {
            int index = Random.Range(0, emptyCells.Count);
            GameObject enemy = enemyGroup.GetRandomEnemy(roomStrength - currentStrength);
            if (enemy == null)
                break;
            currentStrength += GenerateEnemy(enemy, emptyCells[index]);
            emptyCells.RemoveAt(index);
        }
        return 0;
    }

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

    private int CalculateRoomStrength(Room room)
    {
        return Random.Range(25, 75) * room.Cells.Count;
    }
    private int CalculateRoomValue(Room room, int strength)
    {
        return strength + Random.Range(5, 15) * room.Cells.Count;
    }

    public int GenerateItem(GameObject item, Cell cell)
    {
        return ItemEntity.Instantiate(item, cell).value;
    }

    public int GenerateGold(Cell cell)
    {
        int amount = Random.Range(8, 14);
        foreach (ItemEntity itemEntity in cell.ItemEntities)
            if (itemEntity is GoldItem goldItem)
            {
                goldItem.Amount += amount;
                return goldItem.value;
            }
        ItemEntity gold = ItemEntity.Instantiate(GetRandomItem(goldItems), cell);
        ((GoldItem)gold).Amount = amount;
        return gold.value;
    }

    public int GenerateEnemy(GameObject enemy, Cell cell)
    {
        return ((EnemyEntity)CreatureEntity.Instantiate(enemy, cell)).strength;
    }

    public PlayerEntity GeneratePlayer(Cell cell)
    {
        return PlayerEntity.Instantiate(playerPrefab, cell, goldText);
    }

    private T GetRandomItem<T>(List<T> items)
    {
        return items[Random.Range(0, items.Count)];
    }
}