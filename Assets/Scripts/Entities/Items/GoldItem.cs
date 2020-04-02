public class GoldItem : ItemEntity
{
    public int Amount { get; set; }

    public override void Use(PlayerEntity player)
    {
        player.InputController.Inventory.Gold += Amount;
        Destroy();
    }

    public override string GetDescription()
    {
        return $"{Amount} gold";
    }
}