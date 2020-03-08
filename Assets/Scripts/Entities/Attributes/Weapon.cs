using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponItem weaponItem;

    public TextMeshPro text;

    private void Awake()
    {
        UpdateText();
    }

    public void Attack(CreatureEntity creature)
    {
        int damageLeft = weaponItem.value;
        if (creature.Armor is null || creature.Armor.TakeDamage(damageLeft, out damageLeft))
        {
            if (creature.Health.TakeDamage(damageLeft, out damageLeft))
                creature.Die();
        }
        UpdateText();
    }

    public void Equip(WeaponItem weaponItem)
    {
        this.weaponItem = weaponItem;
        UpdateText();
    }

    private void UpdateText()
    {
        if (text != null)
            text.text = weaponItem.value.ToString();
    }
}