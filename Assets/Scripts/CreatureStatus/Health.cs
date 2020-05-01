using System;
using UnityEngine;
using TMPro;

[Serializable]
public class Health
{
    public HealthBar healthBar;
    public TextMeshPro text;

    private Creature parent;

    public void Initialize(Creature creature)
    {
        parent = creature;
        healthBar.OnValueChanged += UpdateText;
        healthBar.OnEmptyBar += creature.Die;
        healthBar.Initialize();
    }

    private void UpdateText()
    {
        if (text != null)
            text.text = healthBar.Value.ToString();
    }
}