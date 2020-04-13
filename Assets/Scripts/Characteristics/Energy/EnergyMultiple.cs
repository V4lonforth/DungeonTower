using System;
using TMPro;

[Serializable]
public class EnergyMultiple : Energy
{
    public int maxValue;
    public float chargeSpeed;

    public TextMeshPro text;
    
    public int Value { get; private set; }

    public override bool Full => Value == maxValue;
    public override bool Empty => Value == 0;

    protected void Awake()
    {
        Value = maxValue;
        UpdateChargeBar();
    }

    private void UpdateText()
    {
        if (text != null)
            text.text = Value.ToString();
    }

    public override void Use()
    {
        if (Full)
            SetCooldown(chargeSpeed);
        Value--;
    }

    protected override void Recharge()
    {
        SetCooldown(chargeSpeed);
        Value++;
    }

    protected override void UpdateChargeBar()
    {
        UpdateText();
        if (Full)
            chargeBar.Fill(1f);
        else
            chargeBar.Fill(1f - (cooldownLeft / cooldown));
    }
}