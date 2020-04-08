using System.Collections.Generic;

public abstract class EquipmentItem : Item
{
    public List<Effect> effects;

    public static WeaponItem Instantiate(EquipmentItem equipmentItem, Cell cell)
    {
        WeaponItem entity = (WeaponItem)Item.Instantiate(equipmentItem, cell);
        for (int i = 0; i < entity.effects.Count; i++)
            entity.effects[i] = Instantiate(entity.effects[i]);
        return entity;
    }

    public void Equip(CreatureEntity creatureEntity)
    {
        foreach (Effect effect in effects)
            effect.ApplyEffect(creatureEntity);
    }
    public void Unequip(CreatureEntity creatureEntity)
    {
        foreach (Effect effect in effects)
            effect.RemoveEffect(creatureEntity);
    }

    public override void Use(PlayerEntity player)
    {
        player.InputController.Inventory.Equip(this);
    }
}