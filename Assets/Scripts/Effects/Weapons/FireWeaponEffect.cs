using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FireWeaponEffect", order = 1)]
public class FireWeaponEffect : Effect
{
    public int extraDamage;

    public override void ApplyEffect(CreatureEntity creatureEntity)
    {
        base.ApplyEffect(creatureEntity);
        creatureEntity.AttackEvent += AttackEffect;
    }

    public override void RemoveEffect(CreatureEntity creatureEntity)
    {
        base.RemoveEffect(creatureEntity);
        creatureEntity.AttackEvent -= AttackEffect;
    }

    private void AttackEffect(CreatureEntity sender, Damage damage)
    {
        damage.Value += extraDamage;
        damage.Type = DamageType.Fire;
    }
}