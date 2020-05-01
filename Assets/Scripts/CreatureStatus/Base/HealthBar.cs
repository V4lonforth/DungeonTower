using System;

[Serializable]
public class HealthBar : ProgressBar
{
    public HealthType healthType;

    public void TakeDamage(Damage damage)
    {
        if (damage.DamageLeft > 0)
        {
            int resultValue = Value - damage.DamageLeft;
            if (resultValue <= 0)
            {
                damage.DamageDealt += damage.DamageLeft + resultValue;
                damage.DamageLeft = -resultValue;
                resultValue = 0;
            }
            else
            {
                damage.DamageDealt += damage.DamageLeft;
                damage.DamageLeft = 0;
            }
            Value = resultValue;
        }
    }

    public void Heal(int heal)
    {
        Value += heal;
    }
    public void Heal(float heal)
    {
        ValueF += heal;
    }
}