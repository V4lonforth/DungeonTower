using UnityEngine;

public abstract class Energy : MonoBehaviour
{
    public ChargeBar chargeBar;

    public bool Rechargable { get; set; }
    public abstract bool Empty { get; }
    public abstract bool Full { get; }

    protected float cooldown;
    protected float cooldownLeft;

    protected void Update()
    {
        if (!Full && Rechargable)
        {
            cooldownLeft -= Time.deltaTime;
            if (cooldownLeft <= 0f)
            {
                cooldownLeft = 0f;
                Recharge();
                UpdateChargeBar();
            }
            else
                UpdateChargeBar();
        }
    }

    public virtual void SetCooldown(float cooldown)
    {
        this.cooldown = cooldownLeft = cooldown;
    }

    public abstract void Use();
    protected abstract void UpdateChargeBar();
    protected abstract void Recharge();
}