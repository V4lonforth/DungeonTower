using UnityEngine;
using TMPro;

public class Armor : MonoBehaviour
{
    public ArmorItem armorItem;

    public TextMeshPro text;

    private PlayerEntity playerEntity;

    private void Awake()
    {
        playerEntity = GetComponent<PlayerEntity>();
        if (armorItem != null)
            armorItem.armor = armorItem.maxArmor;
        UpdateText();
    }

    public void Break()
    {
        if (armorItem != playerEntity.defaultArmour)
        {
            armorItem.ItemEntity.Destroy();
            playerEntity.Inventory.Equip(playerEntity.defaultArmour);
        }
    }

    public void Equip(ArmorItem armorItem)
    {
        this.armorItem = armorItem;
        UpdateText();
    }

    public bool TakeDamage(int damage, out int damageLeft)
    {
        if (armorItem == null || armorItem.maxArmor == 0)
        {
            damageLeft = damage;
            return true;
        }

        armorItem.armor -= damage;
        if (armorItem.armor <= 0)
        {
            damageLeft = -armorItem.armor;
            armorItem.armor = 0;
            Break();
            UpdateText();
            return true;
        }
        damageLeft = 0;
        UpdateText();
        return false;
    }

    public void Repair(int value)
    {
        armorItem.armor = Mathf.Min(armorItem.armor + value, armorItem.maxArmor);
    }
    public void Repair(float value)
    {
        Repair(Mathf.RoundToInt(armorItem.maxArmor * value));
    }

    private void UpdateText()
    {
        if (text != null && armorItem != null)
            text.text = armorItem.armor.ToString();
    }
}