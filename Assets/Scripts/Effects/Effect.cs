using System;
using System.Reflection;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    protected Creature creature;

    protected void Awake()
    {
        if (TryGetComponent(out creature))
        {
            creature.Effects.Add(this);
            ApplyEffect();
        }
    }

    public Effect Apply(Creature creature)
    {
        Type type = GetType();
        Effect effect = (Effect)creature.gameObject.AddComponent(type);

        FieldInfo[] fields = type.GetFields();
        foreach (FieldInfo field in fields)
            if (field.IsPublic)
                field.SetValue(effect, field.GetValue(this));

        return effect;
    }

    public void Remove()
    {
        creature.Effects.Remove(this);
        RemoveEffect();
    }

    protected abstract void ApplyEffect();
    protected abstract void RemoveEffect();
}