using UnityEngine;

public class ArmorEntity : ItemEntity
{
    public ArmorItem armor;

    public static ArmorEntity Instantiate(GameObject prefab, Cell cell, float multiplier = 1f)
    {
        ArmorEntity weaponEntity = (ArmorEntity)Entity.Instantiate(prefab, cell);
        weaponEntity.SetMultiplier(multiplier);
        return weaponEntity;
    }

    public override void SetMultiplier(float multiplier)
    {
        armor.SetMultiplier(multiplier);
    }
}