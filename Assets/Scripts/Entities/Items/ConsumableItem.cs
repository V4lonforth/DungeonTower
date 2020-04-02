public abstract class ConsumableItem : ItemEntity
{
    public int uses;

    public override void Use(PlayerEntity player)
    {
        uses--;
        if (uses <= 0)
            Destroy();

        player.SetTarget(player.Cell);
    }
}