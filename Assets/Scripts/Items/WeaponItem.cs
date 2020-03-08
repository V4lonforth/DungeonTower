public class WeaponItem : Item
{
    public int value;

    public override void Use(PlayerEntity player)
    {
        player.Weapon.Equip(this);
        player.Inventory.Equip(this);
    }
}