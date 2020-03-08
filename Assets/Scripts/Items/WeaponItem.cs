using System;
using UnityEngine;
using TMPro;

[Serializable]
public class WeaponItem : Item
{
    public int damage;

    public WeaponItem(int damage, TextMeshPro text) : base(text)
    {
        this.damage = damage;
        Awake();
    }

    public override Item Clone()
    {
        return new WeaponItem(damage, text);
    }

    public void Awake()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        UpdateText(damage);
    }

    public override void SetMultiplier(float multiplier)
    {
        damage = Mathf.RoundToInt(multiplier * damage);
        UpdateText();
    }

    public void Attack(CreatureEntity creature)
    {
        int damageLeft = damage;
        if (creature.armor is null || creature.armor.TakeDamage(damageLeft, out damageLeft))
        {
            creature.armor = null;
            if (creature.necklace.TakeDamage(damageLeft, out damageLeft))
                creature.Die();
        }
        UpdateText();
    }
}