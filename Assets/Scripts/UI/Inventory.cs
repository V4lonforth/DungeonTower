using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IInteractive
{
    public EquipmentSlot weaponSlot;
    public EquipmentSlot armorSlot;

    public EquipmentSlot[] backpackSlots;

    public int dropOffset;
    public int dropsInterval;
    public RectTransform dropTransform;

    public EquipmentSlot[] dropSlots;

    public PlayerEntity PlayerEntity { get; set; }

    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            UpdateText();
        }
    }

    private List<Item> droppedItemEntities;
    private List<EquipmentSlot> allSlots;

    private int gold;
    private Text goldText;

    private const int BackpackSize = 3;

    private void Awake()
    {
        HideDrop();

        allSlots = new List<EquipmentSlot>();
        allSlots.Add(weaponSlot);
        allSlots.Add(armorSlot);
        allSlots.AddRange(backpackSlots);
        allSlots.AddRange(dropSlots);
    }

    public void SetText(Text goldText)
    {
        this.goldText = goldText;
        UpdateText();
    }

    private void UpdateText()
    {
        if (goldText != null)
            goldText.text = Gold.ToString();
    }

    public void ShowDrop(List<Item> itemEntities, int offset = 0)
    {
        HideDrop();
        if (itemEntities.Count == 0)
            return;
        if (itemEntities.Count == 1)
            itemEntities[0].GetComponent<SpriteRenderer>().enabled = true;

        itemEntities = new List<Item>(itemEntities);
        itemEntities.RemoveAll(itemEntity => itemEntity is ChestItem chest && chest.Opened);
        droppedItemEntities = itemEntities;
        dropTransform.anchoredPosition = new Vector2(dropOffset - dropsInterval * Mathf.Min(itemEntities.Count, dropSlots.Length), 0);
        dropTransform.gameObject.SetActive(true);

        for (int i = 0; i + offset < itemEntities.Count && i < dropSlots.Length; i++)
        {
            dropSlots[i].Show(itemEntities[i + offset]);
        }
    }

    public void HideDrop()
    {
        dropTransform.gameObject.SetActive(false);
        foreach (EquipmentSlot equipmentSlot in dropSlots)
            equipmentSlot.Hide();
    }

    private void Attach(Item item)
    {
        item.DetachFromCell();
        item.transform.SetParent(PlayerEntity.transform);
        item.gameObject.SetActive(false);
    }
    private void Detach(EquipmentSlot equipmentSlot)
    {
        if (!IsEmpty(equipmentSlot))
        {
            equipmentSlot.Item.gameObject.SetActive(true);
            equipmentSlot.Item.AttachToCell(PlayerEntity.Cell);
            equipmentSlot.Item.DetachFromEquipmentSlot();
            equipmentSlot.DetachItem();
        }
    }

    private void RemoveFromDrop(EquipmentSlot equipmentSlot)
    {
        HideDrop();
        ShowDrop(PlayerEntity.Cell.ItemEntities);
    }

    private void DragEquipment(EquipmentSlot from, EquipmentSlot to)
    {
        if (IsEquipped(from))
        {
            if (to.Item != null && from.Item.GetType() != to.Item.GetType())
            {
                if (IsDropped(to))
                    return;
                to = FindEmptyBackpackSlot();
            }
            if (to == null)
                return;
            if (to.Item != null)
            {
                Equip(to.Item);
                return;
            }
        }

        Item fromItem = from.Item;
        Item toItem = to.Item;

        if (IsDropped(from))
        {
            Attach(from.Item);
            Detach(to);
        }
        else if (IsDropped(to))
        {
            Attach(to.Item);
            Detach(from);
        }
        to.Show(fromItem);
        if (toItem == null || toItem == PlayerEntity.defaultWeapon || toItem == PlayerEntity.defaultArmour)
        {
            if (IsDropped(from))
                RemoveFromDrop(from);
            else
                from.DetachItem();
        }
        else
            from.Show(toItem);

        if (from.Item == null)
        {
            if (IsEquippedArmor(from))
                Equip(PlayerEntity.defaultArmour);
            else if (IsEquippedWeapon(from))
                Equip(PlayerEntity.defaultWeapon);
        }
    }

    public void Equip(Item item)
    {
        EquipmentSlot equipmentSlot = FindItem(item);
        EquipmentSlot equippedSlot = null;
        if (item is WeaponItem weapon)
        {
            PlayerEntity.weapon.Equip(weapon);
            equippedSlot = weaponSlot;
        }
        else if (item is ArmorItem armor)
        {
            PlayerEntity.armor.Equip(armor);
            equippedSlot = armorSlot;
        }
        if (equipmentSlot == null)
            equippedSlot.Show(item);
        else
            DragEquipment(equipmentSlot, equippedSlot);
    }

    public bool Press(Vector2 position, int id)
    {
        if (!PlayerEntity.Tower.TurnController.AbleToMakeMove)
            return false;
        foreach (EquipmentSlot equipmentSlot in allSlots)
            if (equipmentSlot.Active && !IsEmpty(equipmentSlot) && equipmentSlot.Press(position, id))
                return true;
        return false;
    }

    public bool Hold(Vector2 position, int id)
    {
        if (!PlayerEntity.Tower.TurnController.AbleToMakeMove)
            return false;
        foreach (EquipmentSlot equipmentSlot in allSlots)
            if (equipmentSlot.Active && equipmentSlot.Hold(position, id))
                return true;
        return false;
    }

    public bool Release(Vector2 position, int id)
    {
        if (!PlayerEntity.Tower.TurnController.AbleToMakeMove)
            return false;
        foreach (EquipmentSlot equipmentSlot in allSlots)
            if (equipmentSlot.Active && equipmentSlot.Release(position, id))
                return true;
        return false;
    }

    public EquipmentSlot FindDropEquipmentSlot(Vector2 position)
    {
        foreach (EquipmentSlot equipmentSlot in allSlots)
            if (equipmentSlot.Active && equipmentSlot.CheckPosition(position))
                return equipmentSlot;
        return null;
    }

    private bool IsDropped(EquipmentSlot equipmentSlot) => dropSlots.Contains(equipmentSlot);
    private bool IsEquippedWeapon(EquipmentSlot equipmentSlot) => equipmentSlot == weaponSlot;
    private bool IsEquippedArmor(EquipmentSlot equipmentSlot) => equipmentSlot == armorSlot;
    private bool IsEquipped(EquipmentSlot equipmentSlot) => IsEquippedArmor(equipmentSlot) || IsEquippedWeapon(equipmentSlot);
    private bool IsInBackpack(EquipmentSlot equipmentSlot) => backpackSlots.Contains(equipmentSlot);
    private bool IsInInventory(EquipmentSlot equipmentSlot) => IsEquipped(equipmentSlot) || IsInBackpack(equipmentSlot);

    private EquipmentSlot FindItem(Item item) => allSlots.Find(slot => slot.Item == item);
    private EquipmentSlot FindEmptyBackpackSlot() => backpackSlots.FirstOrDefault(slot => slot.Item == null);
    private bool IsEmpty(EquipmentSlot equipmentSlot) => IsEmpty(equipmentSlot.Item);
    private bool IsEmpty(Item item) => item == null || item == PlayerEntity.defaultWeapon || item == PlayerEntity.defaultArmour;

    public void TryDropEquipment(EquipmentSlot equipmentSlot)
    {
        if (IsInInventory(equipmentSlot) && !IsEmpty(equipmentSlot))
        {
            Detach(equipmentSlot);

            if (IsEquippedArmor(equipmentSlot))
            {
                if (PlayerEntity.defaultArmour == null)
                {
                    armorSlot.Hide();
                    PlayerEntity.armor.Unequip();
                }
                else
                    Equip(PlayerEntity.defaultArmour);
            }
            else if (IsEquippedWeapon(equipmentSlot))
                Equip(PlayerEntity.defaultWeapon);

            ShowDrop(PlayerEntity.Cell.ItemEntities);
        }
    }

    public void TryDragEquipment(EquipmentSlot from, EquipmentSlot to)
    {
        if (!ReferenceEquals(from, to))
        {
            if (IsEquipped(to))
            {
                if ((from.Item is WeaponItem && IsEquippedWeapon(to)) || (from.Item is ArmorItem && IsEquippedArmor(to)))
                    Equip(from.Item);
            }
            else
            {
                DragEquipment(from, to);
            }
        }
    }

    public void TryPutInInventory(EquipmentSlot equipmentSlot)
    {
        if (IsDropped(equipmentSlot))
        {
            if (equipmentSlot.Item is WeaponItem)
            {
                if (IsEmpty(weaponSlot))
                {
                    Equip(equipmentSlot.Item);
                    return;
                }
            }
            else if (equipmentSlot.Item is ArmorItem)
            {
                if (IsEmpty(armorSlot))
                {
                    Equip(equipmentSlot.Item);
                    return;
                }
            }
            EquipmentSlot emptySlot = FindEmptyBackpackSlot();
            if (emptySlot != null)
                TryDragEquipment(equipmentSlot, emptySlot);
        }
    }

    public void Use(EquipmentSlot equipmentSlot)
    {
        if (IsDropped(equipmentSlot))
        {
            if (equipmentSlot.Item.collectable)
                TryPutInInventory(equipmentSlot);
            else
                equipmentSlot.Item.Use(PlayerEntity);
        }
        else if (IsInBackpack(equipmentSlot))
            equipmentSlot.Item.Use(PlayerEntity);
    }
}