public abstract class ConsumableItem : Item
{
    public int uses;

    public override void Use(PlayerEntity player)
    {
        uses--;
        if (uses <= 0)
            ItemEntity.Destroy();

        player.SetTarget(player.Cell);
    }
}