using System;
using UnityEngine;
using TMPro;

[Serializable]
public abstract class Item
{
    public TextMeshPro text;
    public abstract void SetMultiplier(float multiplier);

    public Item()
    { }

    public Item(TextMeshPro text)
    {
        this.text = text;
    }

    protected virtual void UpdateText(int value)
    {
        if (text)
            text.text = value.ToString();
    }
}