using UnityEngine;

public class ItemEntity : Entity
{
    public Item item;

    public static ItemEntity Instantiate(GameObject prefab, Cell cell, Item item, float multiplier = 1f)
    {
        ItemEntity itemEntity = (ItemEntity)Instantiate(prefab, cell);
        itemEntity.item = item;
        itemEntity.SetMultiplier(multiplier);
        Instantiate(item.prefab, itemEntity.transform);
        return itemEntity;
    }

    public void SetMultiplier(float multiplier)
    {
        item.SetMultiplier(multiplier);
    }
}