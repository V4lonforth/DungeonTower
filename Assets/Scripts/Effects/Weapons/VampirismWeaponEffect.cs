using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/VampirismWeaponEffect", order = 1)]
public class VampirismWeaponEffect : Effect
{
    public float percentage;

    public override void ApplyEffect(CreatureEntity creatureEntity)
    {
        base.ApplyEffect(creatureEntity);
        creatureEntity.PostAttackEvent += AttackEffect;
    }

    public override void RemoveEffect(CreatureEntity creatureEntity)
    {
        base.RemoveEffect(creatureEntity);
        creatureEntity.PostAttackEvent -= AttackEffect;
    }

    private void AttackEffect(Damage damage)
    {
        damage.Attacker.health.health.Heal(Mathf.CeilToInt(damage.DamageDealt * percentage));
    }
}