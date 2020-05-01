using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Slot weaponSlot;
    public Slot armorSlot;

    public Slot[] backpackSlots;

    private List<Slot> playerSlots;
    private List<Slot> allSlots;

    public ItemDrop itemDrop;

    private TowerGenerator towerGenerator;
    public Player Player => towerGenerator.Tower.Player;

    private void Awake()
    {
        towerGenerator = FindObjectOfType<TowerGenerator>();
        itemDrop.Hide();

        playerSlots = new List<Slot>();
        playerSlots.Add(weaponSlot);
        playerSlots.Add(armorSlot);
        playerSlots.AddRange(backpackSlots);

        allSlots = new List<Slot>();
        allSlots.AddRange(playerSlots);
        allSlots.AddRange(itemDrop.dropSlots);
    }

    public void DropItem(Slot slot)
    {
        itemDrop.CurrentItems.Add(slot.Item);
        slot.RemoveItem();
        itemDrop.Show(itemDrop.CurrentItems, itemDrop.CurrentOffset);
    }

    public void SwapItems(Slot first, Slot second)
    {
        Item firstItem = first.Item;
        Item secondItem = second.Item;

        if (first.AbleToPlaceItem(secondItem) && second.AbleToPlaceItem(firstItem))
        {
            first.RemoveItem();
            second.RemoveItem();

            first.SetItem(secondItem);
            second.SetItem(firstItem);
        }
    }

    public void Use(Slot slot)
    {
        if (slot.Item == null)
            return;

        switch (slot.slotType)
        {
            case SlotType.Drop:
                Slot emptySlot = playerSlots.Find(playerSlot => playerSlot.Item == null && playerSlot.AbleToPlaceItem(slot.Item));
                if (emptySlot != null)
                    SwapItems(slot, emptySlot);
                break;
            case SlotType.Backpack:
                if (slot.Item is WeaponItem weapon)
                    SwapItems(slot, weaponSlot);
                else if (slot.Item is ArmorItem armor)
                    SwapItems(slot, armorSlot);
                else
                    slot.Item.Use(Player);
                break;
            case SlotType.WeaponEquipment:
            case SlotType.ArmorEquipment:
                slot.Item.Use(Player);
                break;
        }
    }

    public Slot FindDropEquipmentSlot(Vector2 position)
    {
        foreach (Slot equipmentSlot in allSlots)
            if (equipmentSlot.Active && equipmentSlot.SlotButton.Contains(position))
                return equipmentSlot;
        return null;
    }
}