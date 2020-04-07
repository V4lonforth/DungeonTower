using System;
using TMPro;

[Serializable]
public class Weapon
{
    public WeaponItem weaponItem;

    public TextMeshPro text;

    private CreatureEntity parent;

    public void Awake(CreatureEntity creatureEntity)
    {
        parent = creatureEntity;
        if (weaponItem != null)
            Equip(weaponItem);
    }

    private Damage GetDamage(CreatureEntity target)
    {
        return new Damage(weaponItem.damage, DamageType.Physical, parent, target);
    }

    public void Attack(CreatureEntity creature)
    {
        Damage damage = GetDamage(creature);
        parent.AttackEvent?.Invoke(damage);
        creature.TakeDamage(damage);
        parent.PostAttackEvent?.Invoke(damage);
        UpdateText();
    }

    public void Equip(WeaponItem weaponItem)
    {
        if (this.weaponItem != null)
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