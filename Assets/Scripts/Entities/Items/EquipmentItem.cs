public abstract class EquipmentItem : ItemEntity
{
    public override void Use(PlayerEntity player)
    {
        player.InputController.Inventory.Equip(this);
    }
}