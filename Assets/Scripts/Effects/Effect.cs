using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public bool canExpire;
    public int timeToExpire;

    protected CreatureEntity creatureEntity;

    public virtual void ApplyEffect(CreatureEntity creatureEntity)
    {
        this.creatureEntity = creatureEntity;
    }
    public virtual void RemoveEffect(CreatureEntity creatureEntity)
    {
        creatureEntity.Effects.Remove(this);
    }

    protected void FinishMove()
    {
        if (canExpire)
        {
            timeToExpire--;
            if (timeToExpire <= 0)
                RemoveEffect(creatureEntity);
        }
    }
}