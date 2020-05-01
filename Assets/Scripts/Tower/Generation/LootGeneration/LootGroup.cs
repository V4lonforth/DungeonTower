using System;
using System.Collections.Generic;

[Serializable]
public class LootGroup
{
    [Serializable]
    public class WeightedItem
    {
        public float weight = 1f;
        public Item item;
    }

    public List<WeightedItem> items;

    public Item GetRandomItem(int value)
    {
        return MathHelper.GetRandomElement(items.FindAll(element => element.item.value <= value), item => item.weight)?.item;
    }
}