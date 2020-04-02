public class Damage
{
    public int Value { get; set; }
    public DamageType Type { get; set; }
    public CreatureEntity Creature { get; set; }

    public Damage(int value, DamageType type, CreatureEntity creature)
    {
        Value = value;
        Type = type;
        Creature = creature;
    }
}