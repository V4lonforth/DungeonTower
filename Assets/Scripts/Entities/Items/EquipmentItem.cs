using System.Collections.Generic;

public abstract class EquipmentItem : Item
{
    public List<Effect> effects;

    public static WeaponItem Instantiate(EquipmentItem equipmentItem, Cell cell)
    {
        WeaponItem weapon = (WeaponItem)Item.Instantiate(equipmentItem, cell);
        for (int i = 0; i < weapon.effects.Count; i++)
            weapon.effects[i] = Instantiate(weapon.effects[i]);
        return weapon;
    }

    public void Equip(Creature creature)
    {
        foreach (Effect effect in effects)
            effect.ApplyEffect(creature);
    }
    public void Unequip(Creature creature)
    {
        foreach (Effect effect in effects)
            effect.RemoveEffect(creature);
    }

    public override void Use(Player player)
    {
        player.InputController.Inventory.Equip(this);
    }
}