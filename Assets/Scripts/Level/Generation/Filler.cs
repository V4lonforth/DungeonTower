using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Filler : MonoBehaviour
{
    public Text goldText;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject goldPrefab;
    public GameObject weaponPrefab;
    public GameObject armorPrefab;
    public GameObject necklacePrefab;

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

    public GoldEntity GenerateGold(Cell cell, float multiplier = 1f)
    {
        GoldEntity gold = GoldEntity.Instantiate(goldPrefab, cell, Random.Range(8, 14));
        gold.gold.text = gold.GetComponentInChildren<TextMeshPro>();
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

    public WeaponEntity GenerateWeapon(Cell cell, float multiplier = 1f)
    {
        WeaponEntity weapon = WeaponEntity.Instantiate(weaponPrefab, cell, multiplier);
        weapon.SetMultiplier(multiplier);
        return weapon;
    }

    public ArmorEntity GenerateArmor(Cell cell, float multiplier = 1f)
    {
        ArmorEntity armor = ArmorEntity.Instantiate(armorPrefab, cell, multiplier);
        armor.SetMultiplier(multiplier);
        return armor;
    }

    public NecklaceEntity GenerateNecklace(Cell cell, float multiplier = 1f)
    {
        NecklaceEntity necklace = NecklaceEntity.Instantiate(necklacePrefab, cell, multiplier);
        necklace.SetMultiplier(multiplier);
        return necklace;
    }
}