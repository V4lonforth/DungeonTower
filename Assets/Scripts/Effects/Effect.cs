using System;

[Serializable]
public abstract class Effect
{
    public bool canExpire;
    public int timeToExpire;

    protected CreatureEntity creatureEntity;

    public virtual void AddEffect(CreatureEntity creatureEntity)
    {
        this.creatureEntity = creatureEntity;
    }
    public virtual void RemoveEffect(CreatureEntity creatureEntity)
    {
        creatureEntity.Effects.Remove(this);
    }
    public Effect Clone() => (Effect)MemberwiseClone();

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