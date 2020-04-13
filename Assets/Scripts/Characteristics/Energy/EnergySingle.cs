public class EnergySingle : Energy
{
    public override bool Full => charged;
    public override bool Empty => !charged;

    private bool charged;

    public override void SetCooldown(float cooldown)
    {
        base.SetCooldown(cooldown);
        Use();
    }

    public override void Use()
    {
        charged = false;
    }

    protected override void Recharge()
    {
        charged = true;
    }

    protected override void UpdateChargeBar()
    {
        chargeBar.Fill(1f - (cooldownLeft / cooldown));
    }
}