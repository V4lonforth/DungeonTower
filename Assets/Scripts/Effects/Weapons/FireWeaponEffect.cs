using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FireWeaponEffect", order = 1)]
public class FireWeaponEffect : Effect
{
    public int extraDamage;

    public override void ApplyEffect(Creature creature)
    {
        base.ApplyEffect(creature);
        creature.AttackEvent += AttackEffect;
    }

    public override void RemoveEffect(Creature creature)
    {
        base.RemoveEffect(creature);
        creature.AttackEvent -= AttackEffect;
    }

    private void AttackEffect(Damage damage)
    {
        damage.DamageLeft += extraDamage;
        damage.Type = DamageType.Fire;
    }
}