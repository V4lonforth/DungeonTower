using UnityEngine;
using UnityEngine.UI;

namespace DungeonTower.Level.Shop
{
    public class ItemDisplay : MonoBehaviour
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private Text itemNameText;
        [SerializeField] private Text itemDescriptionText;
        [SerializeField] private Text costText;
        [SerializeField] private Button sellButton;
        
        public void Display(ShopController shopController, SellableItem sellableItem)
        {
            itemIcon.sprite = sellableItem.icon;
            itemNameText.text = sellableItem.itemName;
            itemDescriptionText.text = sellableItem.description;
            costText.text = sellableItem.cost.ToString();

            sellButton.onClick.AddListener(() => shopController.BuyItem(sellableItem));
        }
    }
}