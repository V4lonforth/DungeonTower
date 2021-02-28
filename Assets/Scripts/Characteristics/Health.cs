using System;
using TMPro;

[Serializable]
public class Health
{
    public HealthBar health;

    public TextMeshPro text;

    public void Awake()
    {
        health.ValueChangedEvent += UpdateText;
        health.Initialize();
    }

    private void UpdateText()
    {
        if (text != null)
            text.text = health.Value.ToString();
    }
}