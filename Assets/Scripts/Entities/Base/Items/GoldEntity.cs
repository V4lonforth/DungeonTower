using UnityEngine;

public class GoldEntity : ItemEntity
{
    public GoldItem gold;

    public static GoldEntity Instantiate(GameObject prefab, Cell cell, int amount)
    {
        GoldEntity goldEntity = (GoldEntity)Instantiate(prefab, cell);
        goldEntity.gold.Amount = amount;
        return goldEntity;
    }

    public override void SetMultiplier(float multiplier)
    {
        gold.SetMultiplier(multiplier);
    }
}