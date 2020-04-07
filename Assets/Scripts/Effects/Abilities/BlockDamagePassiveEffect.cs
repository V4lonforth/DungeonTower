using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BlockDamagePassiveEffect", order = 1)]
public class BlockDamagePassiveEffect : Effect
{
    public float probability;

    public override void ApplyEffect(CreatureEntity creatureEntity)
    {
        base.ApplyEffect(creatureEntity);
        creatureEntity.TakeDamageEvent += BlockDamage;
        
    }
    public override void RemoveEffect(CreatureEntity creatureEntity)
    {
        base.RemoveEffect(creatureEntity);
        creatureEntity.TakeDamageEvent -= BlockDamage;
    }

    private void BlockDamage(Damage damage)
    {
        if (damage.Target.Cell.GetDirectionToCell(damage.Attacker.Cell) == damage.Target.FacingDirection && Random.Range(0f, 1f) < probability)
            damage.DamageLeft = 0;
    }
}