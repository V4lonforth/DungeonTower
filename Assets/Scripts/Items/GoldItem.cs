using System;
using UnityEngine;
using TMPro;

[Serializable]
public class GoldItem : Item
{
    public int Amount
    {
        get => amount;
        set
        {
            amount = value;
            UpdateText();
        }
    }
    private int amount;

    public GoldItem(int amount, TextMeshPro text) : base(text)
    {
        Amount = amount;
    }

    private void UpdateText()
    {
        UpdateText(amount);
    }

    public override void SetMultiplier(float multiplier)
    {
        Amount = Mathf.RoundToInt(multiplier * Amount);
        UpdateText();
    }
}