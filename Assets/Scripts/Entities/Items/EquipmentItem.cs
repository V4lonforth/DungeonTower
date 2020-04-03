using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentItem : ItemEntity
{
    public List<Effect> effects;

    public new static WeaponItem Instantiate(GameObject prefab, Cell cell)
    {
        WeaponItem entity = (WeaponItem)ItemEntity.Instantiate(prefab, cell);
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