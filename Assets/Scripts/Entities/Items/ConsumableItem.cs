public abstract class ConsumableItem : Item
{
    public int uses;

    public override void Use(Player player)
    {
        uses--;
        if (uses <= 0)
            Destroy();

        player.SetTarget(player.Cell);
    }
}