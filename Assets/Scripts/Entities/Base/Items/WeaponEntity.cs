using UnityEngine;

public class WeaponEntity : ItemEntity
{
    public WeaponItem weapon;

    public static WeaponEntity Instantiate(GameObject prefab, Cell cell, float multiplier = 1f)
    {
        WeaponEntity weaponEntity = (WeaponEntity)Entity.Instantiate(prefab, cell);
        weaponEntity.SetMultiplier(multiplier);
        return weaponEntity;
    }

    public override void SetMultiplier(float multiplier)
    {
        weapon.SetMultiplier(multiplier);
    }
}