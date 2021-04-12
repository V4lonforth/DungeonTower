using System;
using UnityEngine;

namespace DungeonTower.Level.Shop
{
    [Serializable]
    public class SellableItem : ICloneable
    {
        public string itemName;
        public string description;
        public Sprite icon;
        public int cost;
        
        public GameObject itemPrefab;
        
        public object Clone() => MemberwiseClone();
    }
}