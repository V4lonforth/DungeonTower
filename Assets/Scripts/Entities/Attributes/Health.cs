using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    public int maxValue;
    public int value;

    public TextMeshPro text;

    private void Awake()
    {
        value = maxValue;
        UpdateText();
    }

    public bool TakeDamage(int damage, out int damageLeft)
    {
        value -= damage;
        if (value <= 0)
        {
            damageLeft = -value;
            value = 0;
            UpdateText();
            return true;
        }
        damageLeft = 0;
        UpdateText();
        return false;
    }

    public void Heal(int heal)
    {
        value = Mathf.Min(value + heal, maxValue);
        UpdateText();
    }
    public void Heal(float heal)
    {
        Heal(Mathf.RoundToInt(maxValue * heal));
    }

    private void UpdateText()
    {
        if (text != null)
            text.text = value.ToString();
    }
}