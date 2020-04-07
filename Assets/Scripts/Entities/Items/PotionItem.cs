public class PotionItem : ConsumableItem
{
    public float healStrength;

    public override void Use(PlayerEntity player)
    {
        base.Use(player);
        player.health.health.Heal(healStrength);
    }

    public override string GetDescription()
    {
        return $"Heals {healStrength * 100}% hp";
    }
}