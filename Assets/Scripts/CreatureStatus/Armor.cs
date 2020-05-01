using System;
using TMPro;

[Serializable]
public class Armor
{
    public TextMeshPro text;

    private ArmorItem armorItem;
    private Creature parent;

    public void Initialize(Creature creature)
    {
        parent = creature;
    }

    public void Equip(ArmorItem armorItem)
    {
        this.armorItem = armorItem;
        armorItem.armorBar.OnValueChanged += UpdateText;
        armorItem.armorBar.OnEmptyBar += DestroyArmor;
    }

    public void Unequip()
    {
        armorItem.armorBar.OnValueChanged -= UpdateText;
        armorItem.armorBar.OnEmptyBar -= DestroyArmor;
        armorItem = null;
    }

    public void TakeDamage(Damage damage)
    {
        if (armorItem != null)
            armorItem.armorBar.TakeDamage(damage);
    }

    private void DestroyArmor()
    {
        armorItem.Destroy();
        armorItem = null;
    }

    private void UpdateText()
    {
        if (text != null)
            text.text = armorItem.armorBar.Value.ToString();
    }
}