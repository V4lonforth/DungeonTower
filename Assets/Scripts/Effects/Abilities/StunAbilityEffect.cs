using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StunAbilityEffect", order = 1)]
public class StunAbilityEffect : Effect
{
    public override void ApplyEffect(Creature creature)
    {
        base.ApplyEffect(creature);
        creature.PrepareMoveEvent += StunEvent;
    }

    public override void RemoveEffect(Creature creature)
    {
        base.RemoveEffect(creature);
        creature.PrepareMoveEvent -= StunEvent;
    }

    private void StunEvent(Creature sender, Cell target)
    {
        FinishMove();
    }
}