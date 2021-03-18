using DungeonTower.Entity.Health;
using DungeonTower.Entity.Items;
using DungeonTower.Level.Base;

public class HealingPotionConsumable : Consumable
{
    public int healStrength;

    protected override void Use(Cell target)
    {
        target.FrontEntity.GetComponent<EntityHealth>().RestoreHealth(healStrength);
    }
}