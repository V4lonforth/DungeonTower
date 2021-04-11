using System;
using System.Collections.Generic;
using DungeonTower.Controllers;
using DungeonTower.Entity.Base;
using DungeonTower.Entity.Interactable;
using DungeonTower.Entity.Items;
using DungeonTower.Inventory;
using DungeonTower.Level.Base;
using DungeonTower.Utils;
using UnityEngine;

namespace DungeonTower.Level.Shop
{
    public class ShopController : Singleton<ShopController>
    {
        [SerializeField] private GameObject shop;
        [SerializeField] private Transform itemListTransform;
        
        [SerializeField] private GameObject itemDisplayPrefab;
        
        [SerializeField] private ItemController itemController;
        [SerializeField] private InventoryController inventoryController;

        private List<SellableItem> sellableItems;
        private List<GameObject> itemDisplays = new List<GameObject>();

        private ShopInteractable shopInteractable;
        private CellEntity playerEntity;

        private void Awake()
        {
            shop.SetActive(false);

            GameController.Instance.OnStageStart += StartStage;
        }

        private void StartStage(Stage stage)
        {
            playerEntity = stage.PlayerEntity;
        }

        public void ShowShop(ShopInteractable shopInteractable, List<SellableItem> selectedItems)
        {
            sellableItems = selectedItems;
            this.shopInteractable = shopInteractable;

            shop.SetActive(true);
            
            selectedItems.ForEach(CreateItemDisplay);
        }

        public void CloseShop()
        {
            shop.SetActive(false);
            
            itemDisplays.ForEach(Destroy);
        }

        private void CreateItemDisplay(SellableItem sellableItem)
        {
            GameObject itemDisplay = Instantiate(itemDisplayPrefab, itemListTransform);
            itemDisplay.GetComponent<ItemDisplay>().Display(this, sellableItem);
            itemDisplays.Add(itemDisplay);
        }

        public void BuyItem(SellableItem sellableItem)
        {
            if (MoneyController.Instance.Money >= sellableItem.cost)
            {
                MoneyController.Instance.RemoveMoney(sellableItem.cost);
                shopInteractable.RemoveItem(sellableItem);
                CloseShop();
                ShowShop(shopInteractable, sellableItems);

                LootItem lootItem = itemController.CreateItem(sellableItem.itemPrefab);
                inventoryController.PickupItem(lootItem);
            }
        }
    }
}