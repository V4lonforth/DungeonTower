using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public bool canExpire;
    public int timeToExpire;

    protected Creature creature;

    public virtual void ApplyEffect(Creature creature)
    {
        this.creature = creature;
    }
    public virtual void RemoveEffect(Creature creature)
    {
        creature.Effects.Remove(this);
    }

    protected void FinishMove()
    {
        if (canExpire)
        {
            timeToExpire--;
            if (timeToExpire <= 0)
                RemoveEffect(creature);
        }
    }
}