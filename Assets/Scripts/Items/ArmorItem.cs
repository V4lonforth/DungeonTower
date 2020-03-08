using System;
using UnityEngine;
using TMPro;

[Serializable]
public class ArmorItem : Item
{
    public int armor;

    public ArmorItem(int armor, TextMeshPro text) : base(text)
    {
        this.armor = armor;
        Awake();
    }

    public override Item Clone()
    {
        return new ArmorItem(armor, text);
    }

    public void Awake()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        UpdateText(armor);
    }

    public override void SetMultiplier(float multiplier)
    {
        armor = Mathf.RoundToInt(multiplier * armor);
        UpdateText();
    }

    public bool TakeDamage(int damage, out int damageLeft)
    {
        armor -= damage;
        if (armor <= 0)
        {
            damageLeft = -armor;
            armor = 0;
            UpdateText();
            return true;
        }
        damageLeft = 0;
        UpdateText();
        return false;
    }
}