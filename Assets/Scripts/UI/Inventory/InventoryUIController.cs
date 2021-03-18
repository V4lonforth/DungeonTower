using DungeonTower.Controllers;
using DungeonTower.Inventory;
using DungeonTower.Level.Base;
using DungeonTower.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.UI.Inventory
{
    public class InventoryUIController : MonoBehaviour
    {
        public Action<Slot> OnSlotSelection { get; set; }
        public Action<Slot, Slot> OnSlotSwap { get; set; }

        [SerializeField] private InventoryController inventoryController;

        private readonly List<Slot> slots = new List<Slot>();
        private Stage stage;

        private void Awake()
        {
            inventoryController.OnSlotAdded += AddSlot;
            inventoryController.OnSlotRemoved += RemoveSlot;

            GameController.Instance.OnStageStart += s => stage = s;
        }

        private void AddSlot(Slot slot)
        {
            slots.Add(slot);
            slot.SlotItem.OnSelect += SelectSlot;
            slot.SlotItem.OnDrop += DropSlotItem;
            slot.SlotItem.OnDrag += DragSlot;
        }

        private void RemoveSlot(Slot slot)
        {
            slot.SlotItem.OnSelect -= SelectSlot;
            slot.SlotItem.OnDrop -= DropSlotItem;
            slot.SlotItem.OnDrag -= DragSlot;
            slots.Remove(slot);
        }

        private void SelectSlot(Slot slot)
        {
            inventoryController.PressSlot(slot);
        }

        private void DragSlot(Slot slot)
        {
            inventoryController.DragSlot(slot);
        }

        private void DropSlotItem(Slot selectedSlot, Vector2 position)
        {
            foreach (Slot dropSlot in slots)
            {
                if (dropSlot != selectedSlot && dropSlot.Contains(position))
                {
                    inventoryController.SwapItems(selectedSlot, dropSlot);
                    return;
                }
            }

            Cell cell = stage.GetCellSafe(stage.WorldToTowerPoint(CameraController.Instance.Camera.ScreenToWorldPoint(position)));
            if (!inventoryController.DropOnCell(cell))
                inventoryController.DeselectSlot();
        }
    }
}
