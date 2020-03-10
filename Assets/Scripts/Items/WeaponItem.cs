public class WeaponItem : Item
{
    public int value;

    public override void Use(PlayerEntity player)
    {
        player.Inventory.Equip(this);
    }

    public override string GetDescription()
    {
        return $"{value} damage";
    }
}