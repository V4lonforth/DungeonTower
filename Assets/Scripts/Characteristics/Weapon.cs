using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponItem weaponItem;

    public TextMeshPro text;

    private CreatureEntity parent;

    private void Awake()
    {
        UpdateText();
        parent = GetComponent<CreatureEntity>();
    }

    private Damage GetDamage()
    {
        return new Damage(weaponItem.damage, DamageType.Physical, parent);
    }

    public void Attack(CreatureEntity creature)
    {
        Damage damage = GetDamage();
        parent.AttackEvent?.Invoke(parent, damage);
        creature.TakeDamage(damage);
        parent.PostAttackEvent?.Invoke(parent, damage);
        UpdateText();
    }

    public void Equip(WeaponItem weaponItem)
    {
        if (weaponItem != null)
            this.weaponItem.Unequip(parent);
        this.weaponItem = weaponItem;
        weaponItem.Equip(parent);
        UpdateText();
    }

    private void UpdateText()
    {
        if (text != null)
            text.text = weaponItem.damage.ToString();
    }
}