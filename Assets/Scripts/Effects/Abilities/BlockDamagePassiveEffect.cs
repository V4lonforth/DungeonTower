using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BlockDamagePassiveEffect", order = 1)]
public class BlockDamagePassiveEffect : Effect
{
    public float probability;

    public override void ApplyEffect(Creature creature)
    {
        base.ApplyEffect(creature);
        creature.TakeDamageEvent += BlockDamage;
        
    }
    public override void RemoveEffect(Creature creature)
    {
        base.RemoveEffect(creature);
        creature.TakeDamageEvent -= BlockDamage;
    }

    private void BlockDamage(Damage damage)
    {
        if (damage.Target.Cell.GetDirectionToCell(damage.Attacker.Cell) == damage.Target.FacingDirection && Random.Range(0f, 1f) < probability)
            damage.DamageLeft = 0;
    }
}