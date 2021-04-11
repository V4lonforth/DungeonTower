using System.Collections.Generic;
using DungeonTower.Entity.Base;
using DungeonTower.Level.Shop;
using DungeonTower.Utils.Weights;
using UnityEngine;

namespace DungeonTower.Entity.Interactable
{
    public class ShopInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private WeightedList<SellableItem> sellableItems;
        [SerializeField] private int itemAmountToSell;

        private List<SellableItem> selectedItems;
        
        public bool CanInteract => true;
        
        public void Interact(CellEntity cellEntity)
        {
            if (selectedItems == null)
                GenerateItems();
            
            ShopController.Instance.ShowShop(this, selectedItems);
        }

        private void GenerateItems()
        {
            selectedItems = new List<SellableItem>();
            for (int i = 0; i < itemAmountToSell; i++)
                selectedItems.Add((SellableItem) sellableItems.GetRandom().Clone());
        }

        public void RemoveItem(SellableItem soldItem)
        {
            selectedItems.Remove(soldItem);
        }
    }
}