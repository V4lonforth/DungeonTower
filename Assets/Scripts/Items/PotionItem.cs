public class PotionItem : Item
{
    public float value;

    public override void Use(PlayerEntity player)
    {
        player.Health.Heal(value);
        ItemEntity.Destroy();
    }
}