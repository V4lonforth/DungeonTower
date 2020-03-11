public class PotionItem : ConsumableItem
{
    public float value;

    public override void Use(PlayerEntity player)
    {
        base.Use(player);
        player.Health.Heal(value);
    }

    public override string GetDescription()
    {
        return $"Heals {value * 100}% hp";
    }
}