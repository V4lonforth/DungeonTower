using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/VampirismWeaponEffect", order = 1)]
public class VampirismWeaponEffect : Effect
{
    public float percentage;

    public override void ApplyEffect(Creature creature)
    {
        base.ApplyEffect(creature);
        creature.PostAttackEvent += AttackEffect;
    }

    public override void RemoveEffect(Creature creature)
    {
        base.RemoveEffect(creature);
        creature.PostAttackEvent -= AttackEffect;
    }

    private void AttackEffect(Damage damage)
    {
        damage.Attacker.health.health.Heal(Mathf.CeilToInt(damage.DamageDealt * percentage));
    }
}