using System;
using TMPro;
using UnityEngine;

[Serializable]
public abstract class Item
{
    public GameObject prefab;
    public TextMeshPro text;

    public Item()
    { }

    public Item(TextMeshPro text)
    {
        this.text = text;
    }

    public abstract void SetMultiplier(float multiplier);
    public abstract Item Clone();

    protected virtual void UpdateText(int value)
    {
        if (text)
            text.text = value.ToString();
    }
}