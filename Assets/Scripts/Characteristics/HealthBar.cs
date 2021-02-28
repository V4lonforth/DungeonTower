using System;
using UnityEngine;

[Serializable]
public class HealthBar
{
    public int maxValue;
    public DamageType healthType;

    public int Value { get; private set; }
    public bool Destroyed => Value <= 0;

    public Action ValueChangedEvent;

    public void Initialize()
    {
        Initialize(maxValue, healthType);
    }

    public void Initialize(int maxValue, DamageType healthType)
    {
        Initialize(maxValue, maxValue, healthType);
    }

    public void Initialize(int maxValue, int value, DamageType healthType)
    {
        this.maxValue = maxValue;
        this.healthType = healthType;
        Value = value;
        ValueChangedEvent?.Invoke();
    }

    public void TakeDamage(Damage damage)
    {
        if (damage.DamageLeft > 0)
        {
            Value -= damage.DamageLeft;
            if (Value <= 0)
            {
                damage.DamageDealt += damage.DamageLeft + Value;
                damage.DamageLeft = -Value;
                Value = 0;
            }
            else
            {
                damage.DamageDealt += damage.DamageLeft;
                damage.DamageLeft = 0;
            }
            ValueChangedEvent?.Invoke();
        }
    }

    public void Heal(int heal)
    {
        Value = Mathf.Min(Value + heal, maxValue);
        ValueChangedEvent?.Invoke();
    }
    public void Heal(float heal)
    {
        Heal(Mathf.RoundToInt(maxValue * heal));
        ValueChangedEvent?.Invoke();
    }
}