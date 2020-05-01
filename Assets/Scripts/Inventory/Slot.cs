using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Inventory inventory;
    public Image itemIcon;

    public SlotType slotType;
    public Sprite emptySlotIcon;

    public SlotButton SlotButton { get; private set; }

    public Item Item { get; set; }
    public bool Active { get; private set; }

    private void Awake()
    {
        Active = true;
        SlotButton = GetComponent<SlotButton>();
    }

    public bool AbleToPlaceItem(Item item)
    {
        if (!Active)
            return false;
        switch (slotType)
        {
            case SlotType.ArmorEquipment:
                return item is ArmorItem;
            case SlotType.WeaponEquipment:
                return item is WeaponItem;
        }
        return true;
    }

    public void AttachItem(Item item)
    {
        Active = true;
        Item = item;
        itemIcon.sprite = Item.icon;
    }

    public void DetachItem()
    {
        Active = false;
        Item = null;
        itemIcon.sprite = emptySlotIcon;
    }

    public void SetItem(Item item)
    {
        if (item == null)
            return;

        AttachItem(item);
        Item.Attach(this);

        if (slotType == SlotType.ArmorEquipment && item is ArmorItem armor)
            inventory.Player.armor.Equip(armor);
        else if (slotType == SlotType.WeaponEquipment && item is WeaponItem weapon)
            inventory.Player.weapon.Equip(weapon);
    }

    public void RemoveItem()
    {
        if (Item == null)
            return;

        Item.DetachFromSlot();
        DetachItem();

        if (slotType == SlotType.ArmorEquipment)
            inventory.Player.armor.Unequip();
        else if (slotType == SlotType.WeaponEquipment)
            inventory.Player.weapon.Unequip();
    }
}