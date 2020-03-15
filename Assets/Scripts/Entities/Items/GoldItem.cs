public class GoldItem : Item
{
    public int Amount { get; set; }

    public override void Use(PlayerEntity player)
    {
        player.InputController.Inventory.Gold += Amount;
        ItemEntity.Destroy();
    }

    public override string GetDescription()
    {
        return $"{Amount} gold";
    }
}