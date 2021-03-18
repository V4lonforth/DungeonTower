using DungeonTower.Entity.Items;
using DungeonTower.UI.Inventory;
using UnityEngine;

namespace DungeonTower.Inventory
{
    public class Slot : MonoBehaviour
    {
        public LootItem LootItem { get; private set; }
        public SlotItem SlotItem { get; private set; }

        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            SlotItem = GetComponentInChildren<SlotItem>();

            SlotItem.AttachToSlot(this);
            SlotItem.Image.enabled = false;
        }

        public void AttachItem(LootItem lootItem)
        {
            LootItem = lootItem;
            SlotItem.Image.enabled = true;
            SlotItem.AttachItem(lootItem);
        }

        public void DetachItem()
        {
            LootItem = null;
            SlotItem.Image.enabled = false;
            SlotItem.DetachItem();
        }

        public bool Contains(Vector2 position)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, position);
        }
    }
}
