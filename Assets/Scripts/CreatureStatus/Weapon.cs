using System;
using UnityEngine;
using TMPro;

[Serializable]
public class Weapon
{
    public Damage defaultDamage;
    public GameObject attackAnimationPrefab;
    public TextMeshPro text;

    private WeaponItem weaponItem;
    private Creature parent;

    public void Initialize(Creature creature)
    {
        parent = creature;
    }

    private void UpdateText()
    {
        if (text != null)
            text.text = weaponItem.durability.Value.ToString();
    }

    public void Equip(WeaponItem weaponItem)
    {
        this.weaponItem = weaponItem;
        weaponItem.durability.OnValueChanged += UpdateText;
    }

    public void Unequip()
    {
        weaponItem.durability.OnValueChanged -= UpdateText;
        weaponItem = null;
    }

    public bool CanAttack(Target target)
    {
        return target.Cell != null && target.Cell.Entity is Creature && parent.Cell.IsConnected(target.Cell);
    }

    public void Attack(Target target)
    {
        if (target.Cell != null && target.Cell.Entity is Creature creature)
        {
            AttackAnimation attackAnimation = GameObject.Instantiate(attackAnimationPrefab).GetComponent<AttackAnimation>();
            attackAnimation.SetTarget(creature.transform);

            Damage damage;
            if (weaponItem == null)
            {
                damage = defaultDamage.CreateInstance(parent, creature);
            }
            else
            {
                damage = weaponItem.damage.CreateInstance(parent, creature);
                weaponItem.durability.Value -= weaponItem.degradeSpeed;
            }
            creature.TakeDamage(damage);
        }
    }
}