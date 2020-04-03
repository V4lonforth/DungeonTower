using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StunAbilityEffect", order = 1)]
public class StunAbilityEffect : Effect
{
    public override void ApplyEffect(CreatureEntity creatureEntity)
    {
        base.ApplyEffect(creatureEntity);
        creatureEntity.PrepareMoveEvent += StunEvent;
    }

    public override void RemoveEffect(CreatureEntity creatureEntity)
    {
        base.RemoveEffect(creatureEntity);
        creatureEntity.PrepareMoveEvent -= StunEvent;
    }

    private void StunEvent(CreatureEntity sender, Cell target)
    {
        sender.SkipTurn = true;
        FinishMove();
    }
}