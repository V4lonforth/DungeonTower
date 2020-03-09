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
            armorItem.value = armorItem.maxValue;
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
        if (armorItem == null || armorItem.maxValue == 0)
        {
            damageLeft = damage;
            return true;
        }

        armorItem.value -= damage;
        if (armorItem.value <= 0)
        {
            damageLeft = -armorItem.value;
            armorItem.value = 0;
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
        armorItem.value = Mathf.Min(armorItem.value + value, armorItem.maxValue);
    }
    public void Repair(float value)
    {
        Repair(Mathf.RoundToInt(armorItem.maxValue * value));
    }

    private void UpdateText()
    {
        if (text != null && armorItem != null)
            text.text = armorItem.value.ToString();
    }
}