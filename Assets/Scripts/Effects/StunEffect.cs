using System;

[Serializable]
public class StunEffect : Effect
{
    public override void AddEffect(CreatureEntity creatureEntity)
    {
        base.AddEffect(creatureEntity);
        creatureEntity.PrepareMoveEvent += StunEvent;
    }

    public override void RemoveEffect(CreatureEntity creatureEntity)
    {
        base.RemoveEffect(creatureEntity);
        creatureEntity.PrepareMoveEvent -= StunEvent;
    }

    protected void StunEvent(CreatureEntity sender, Cell target)
    {
        sender.SkipTurn = true;
        FinishMove();
    }
}