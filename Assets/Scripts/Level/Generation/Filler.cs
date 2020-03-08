using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Filler : MonoBehaviour
{
    public TextMeshPro goldText;

    public List<GameObject> armorItems;
    public List<GameObject> potionItems;
    public List<GameObject> weaponItems;
    public List<GameObject> goldItems;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public void Fill(Tower tower)
    {
        tower.Filler = this;
        foreach (Cell cell in tower.Cells)
        {
            float value = Random.Range(0f, 1f);
            if (value < 0.15f)
                GenerateGold(cell);
            else if (value < 0.35f)
                GenerateEnemy(cell);
            else if (value < 0.37f)
                Entity.Instantiate(GetRandomItem(weaponItems), cell);
            else if (value < 0.39f)
                Entity.Instantiate(GetRandomItem(potionItems), cell);
            else if (value < 0.41f)
                Entity.Instantiate(GetRandomItem(armorItems), cell);
        }
        tower[0, 0].Entity?.Destroy();
        tower.Player = GeneratePlayer(tower[0, 0]);
    }

    public ItemEntity GenerateGold(Cell cell)
    {
        ItemEntity gold = (ItemEntity)Entity.Instantiate(GetRandomItem(goldItems), cell);
        ((GoldItem)gold.Item).Amount = Random.Range(8, 14);
        return gold;
    }

    public EnemyEntity GenerateEnemy(Cell cell)
    {
        return (EnemyEntity)Entity.Instantiate(enemyPrefab, cell);
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