using System;
using UnityEngine;

[Serializable]
public class StunEffect : Effect
{
    public override void AddEffect(CreatureEntity creatureEntity)
    {
        base.AddEffect(creatureEntity);
        creatureEntity.MakeMoveEvent += StunEvent;
    }

    public override void RemoveEffect(CreatureEntity creatureEntity)
    {
        base.RemoveEffect(creatureEntity);
        creatureEntity.MakeMoveEvent -= StunEvent;
    }

    protected void StunEvent(CreatureEntity sender, Cell target)
    {
        Debug.Log("Stunned");
        FinishMove();
    }
}