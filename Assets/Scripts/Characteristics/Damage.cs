public class Damage
{
    public int StartDamage { get; set; }
    public int DamageDealt { get; set; }
    public int DamageLeft { get; set; }
    public DamageType Type { get; set; }
    public Creature Attacker { get; set; }
    public Creature Target { get; set; }

    public Damage(int damage, DamageType type, Creature sender, Creature target)
    {
        StartDamage = DamageLeft = damage;
        Type = type;
        Attacker = sender;
        Target = target;
    }
}