using System;
using UnityEngine;
using TMPro;

[Serializable]
public class NecklaceItem : Item
{
    public int maxHealth;
    private int health;

    public NecklaceItem(int maxHealth, TextMeshPro text) : base(text)
    {
        this.maxHealth = maxHealth;
        Awake();
    }

    public override Item Clone()
    {
        return new NecklaceItem(maxHealth, text);
    }

    public void Awake()
    {
        health = maxHealth;
        UpdateText();
    }

    private void UpdateText()
    {
        UpdateText(health);
    }

    public override void SetMultiplier(float multiplier)
    {
        maxHealth = Mathf.RoundToInt(multiplier * maxHealth);
        health = maxHealth;
        UpdateText();
    }

    public bool TakeDamage(int damage, out int damageLeft)
    {
        health -= damage;
        if (health <= 0)
        {
            damageLeft = -health;
            health = 0;
            UpdateText();
            return true;
        }
        damageLeft = 0;
        UpdateText();
        return false;
    }

    public void Heal(int value)
    {
        health = Mathf.Min(health + value, maxHealth);
    }
}