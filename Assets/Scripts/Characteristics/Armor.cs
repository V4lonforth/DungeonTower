using System;
using TMPro;

[Serializable]
public class Armor
{
    public ArmorItem armorItem;
    public TextMeshPro text;

    public bool Equipped => armorItem != null;
    public bool Destroyed => !Equipped || armorItem.armor.Destroyed;

    private Creature parent;

    public void Awake(Creature creature)
    {
        parent = creature;
        if (armorItem != null)
            Equip(armorItem);
    }

    public void Break()
    {
        if (parent is Player player)
        {
            if (armorItem != player.defaultArmour)
            {
                armorItem.Destroy();
                player.InputController.Inventory.Equip(player.defaultArmour);
            }
        }
    }

    public void Equip(ArmorItem armorItem)
    {
        if (this.armorItem != null)
            Unequip();
        this.armorItem = armorItem;
        armorItem.armor.ValueChangedEvent += UpdateText;
        UpdateText();
    }

    public void Unequip()
    {
        armorItem.armor.ValueChangedEvent -= UpdateText;
        armorItem = null;
        UpdateText();
    }

    public void TakeDamage(Damage damage)
    {
        if (Equipped)
        {
            armorItem.armor.TakeDamage(damage);
            if (armorItem.armor.Destroyed)
            {
                armorItem.Destroy();
                armorItem = null;
            }
        }
    }
    
    private void UpdateText()
    {
        if (text != null)
        {
            if (armorItem != null)
                text.text = armorItem.armor.Value.ToString();
            else
                text.text = "0";
        }
    }
}