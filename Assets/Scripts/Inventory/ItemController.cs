using DungeonTower.Entity.Base;
using DungeonTower.Entity.Interactable;
using DungeonTower.Entity.Items;
using DungeonTower.Level.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DungeonTower.Inventory
{
    [CreateAssetMenu(fileName = "Data", menuName = "TowerGeneration/ItemController", order = 1)]
    public class ItemController : ScriptableObject
    {
        [SerializeField] private GameObject itemBagPrefab;

        public Action<LootItem, Cell> OnItemDrop { get; set; }
        public Action<LootItem, Cell> OnItemPickup { get; set; }

        private const string ItemBagName = "ItemBag";

        public List<LootItem> GetItemEntities(Cell cell)
        {
            List<LootItem> lootItems = new List<LootItem>();
            foreach (BackgroundEntity backgroundEntity in cell.BackEntities)
            {
                LootItem lootItem = backgroundEntity.GetComponent<LootItem>();
                if (lootItem != null)
                {
                    lootItems.Add(lootItem);
                }
            }
            return lootItems;
        }

        public LootItem CreateItem(GameObject prefab)
        {
            return Instantiate(prefab).GetComponent<LootItem>();
        }

        public void DropItem(LootItem itemEntity, Cell cell)
        {
            if (!HasItems(cell))
            {
                CreateItemBag(cell);
            }

            itemEntity.Drop(cell);
            OnItemDrop?.Invoke(itemEntity, cell);
        }

        public void PickupItem(LootItem itemEntity, Cell cell, CellEntity cellEntity)
        {
            itemEntity.Pickup(cellEntity);
            OnItemPickup?.Invoke(itemEntity, cell);

            if (!HasItems(cell))
            {
                DeleteItemBag(cell);
            }
        }

        private void CreateItemBag(Cell cell)
        {
            if (CanHaveItemBag(cell))
            {
                GameObject itemBag = Instantiate(itemBagPrefab, cell.Transform);
                itemBag.name = ItemBagName;
            }
        }

        private void DeleteItemBag(Cell cell)
        {
            if (CanHaveItemBag(cell))
            {
                GameObject itemBag = cell.Transform.Find(ItemBagName).gameObject;
                Destroy(itemBag);
            }
        }

        private bool CanHaveItemBag(Cell cell)
        {
            return !cell.BackEntities.Any(e => e.GetComponent<ChestInteractable>());
        }

        private bool HasItems(Cell cell)
        {
            return cell.BackEntities.Any(e => e.GetComponent<LootItem>() != null);
        }
    }
}
