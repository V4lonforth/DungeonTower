public abstract class EquipmentItem : Item
{
    public override void Use(PlayerEntity player)
    {
        player.Inventory.Equip(this);
    }
}