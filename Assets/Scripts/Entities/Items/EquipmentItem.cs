public abstract class EquipmentItem : Item
{
    public override void Use(PlayerEntity player)
    {
        player.InputController.Inventory.Equip(this);
    }
}