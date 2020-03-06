using UnityEngine;

public class NecklaceEntity : ItemEntity
{
    public NecklaceItem necklace;

    public static NecklaceEntity Instantiate(GameObject prefab, Cell cell, float multiplier = 1f)
    {
        NecklaceEntity necklaceEntity = (NecklaceEntity)Entity.Instantiate(prefab, cell);
        necklaceEntity.SetMultiplier(multiplier);
        return necklaceEntity;
    }

    public override void SetMultiplier(float multiplier)
    {
        necklace.SetMultiplier(multiplier);
    }
}