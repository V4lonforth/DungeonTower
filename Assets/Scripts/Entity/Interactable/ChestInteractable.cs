using DungeonTower.Entity.Base;
using DungeonTower.Entity.Items;
using DungeonTower.Inventory;
using DungeonTower.Utils.Weights;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.Entity.Interactable
{
    public class ChestInteractable : ExpendableInteractable
    {
        [SerializeField] private WeightedList<GameObject> items;
        [SerializeField] private Sprite openedChest;

        private ItemController itemController;

        public void Initialize(ItemController itemController)
        {
            this.itemController = itemController;
        }

        protected override void ExpandableInteract(CellEntity cellEntity)
        {
            GetComponent<SpriteRenderer>().sprite = openedChest;
            CellEntity chestCellEntity = GetComponent<CellEntity>();

            foreach (GameObject gameObject in GetItemsToSpawn())
            {
                LootItem lootItem = itemController.CreateItem(gameObject);
                itemController.DropItem(lootItem, chestCellEntity.Cell);
            }
        }

        private List<GameObject> GetItemsToSpawn()
        {
            return new List<GameObject>() { items.GetRandom(), items.GetRandom(), items.GetRandom() };
        }
    }
}
