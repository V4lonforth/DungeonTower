using System;

[Serializable]
public class Damage
{
    public DamageType type;
    public int startDamage;

    public int DamageDealt { get; set; }
    public int DamageLeft { get; set; }
    public Creature Attacker { get; private set; }
    public Creature Target { get; private set; }

    public Damage(DamageType type, int damage, Creature sender, Creature target)
    {
        this.type = type;
        startDamage = DamageLeft = damage;
        Attacker = sender;
        Target = target;
    }

    public Damage CreateInstance(Creature sender, Creature target)
    {
        return new Damage(type, startDamage, sender, target);
    }
}