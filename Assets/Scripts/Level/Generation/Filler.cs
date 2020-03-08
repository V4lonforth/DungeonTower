using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Filler : MonoBehaviour
{
    public Text goldText;

    public List<ArmorItem> armorItems;
    public List<NecklaceItem> necklaceItems;
    public List<WeaponItem> weaponItems;
    public List<GoldItem> goldItems;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject itemPrefab;

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
                GenerateWeapon(cell);
            else if (value < 0.39f)
                GenerateNecklace(cell);
            else if (value < 0.41f)
                GenerateArmor(cell);
        }
        tower[0, 0].Entity?.Destroy();
        tower.Player = GeneratePlayer(tower[0, 0]);
    }

    public ItemEntity GenerateGold(Cell cell, float multiplier = 1f)
    {
        int amount = Random.Range(8, 14);
        GoldItem goldItem = GetRandomItem(goldItems);
        goldItem.Amount = amount;
        ItemEntity gold = ItemEntity.Instantiate(itemPrefab, cell, goldItem, multiplier);
        ((GoldItem)gold.item).text = gold.GetComponentInChildren<TextMeshPro>();
        gold.SetMultiplier(multiplier);
        return gold;
    }

    public EnemyEntity GenerateEnemy(Cell cell, float multiplier = 1f)
    {
        EnemyEntity enemy = (EnemyEntity)Entity.Instantiate(enemyPrefab, cell);
        enemy.SetMultiplier(multiplier);
        return enemy;
    }

    public PlayerEntity GeneratePlayer(Cell cell)
    {
        return PlayerEntity.Instantiate(playerPrefab, cell, goldText);
    }

    public ItemEntity GenerateWeapon(Cell cell, float multiplier = 1f)
    {
        ItemEntity weapon = ItemEntity.Instantiate(itemPrefab, cell, GetRandomItem(weaponItems), multiplier);
        weapon.SetMultiplier(multiplier);
        return weapon;
    }

    public ItemEntity GenerateArmor(Cell cell, float multiplier = 1f)
    {
        ItemEntity armor = ItemEntity.Instantiate(itemPrefab, cell, GetRandomItem(armorItems), multiplier);
        armor.SetMultiplier(multiplier);
        return armor;
    }

    public ItemEntity GenerateNecklace(Cell cell, float multiplier = 1f)
    {
        ItemEntity necklace = ItemEntity.Instantiate(itemPrefab, cell, GetRandomItem(necklaceItems), multiplier);
        necklace.SetMultiplier(multiplier);
        return necklace;
    }

    private T GetRandomItem<T>(List<T> items)
    {
        return items[Random.Range(0, items.Count)];
    }
}