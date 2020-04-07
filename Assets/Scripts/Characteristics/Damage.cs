public class Damage
{
    public int StartDamage { get; set; }
    public int DamageDealt { get; set; }
    public int DamageLeft { get; set; }
    public DamageType Type { get; set; }
    public CreatureEntity Attacker { get; set; }
    public CreatureEntity Target { get; set; }

    public Damage(int damage, DamageType type, CreatureEntity sender, CreatureEntity target)
    {
        StartDamage = DamageLeft = damage;
        Type = type;
        Attacker = sender;
        Target = target;
    }
}