using DungeonTower.Controllers;
using DungeonTower.Entity.Action;
using DungeonTower.Entity.Base;
using DungeonTower.Entity.Items;
using DungeonTower.Entity.Movement;
using DungeonTower.Level.Base;
using DungeonTower.UI.ButtonPanels;
using System;
using System.Collections.Generic;
using DungeonTower.Entity.MoveControllers;
using UnityEngine;

namespace DungeonTower.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private ButtonPanel inventoryPanel;
        [SerializeField] private ButtonPanel lootPanel;

        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private ItemController itemController;
        [SerializeField] private int maxInventorySize;

        private readonly List<Slot> inventorySlots = new List<Slot>();
        private readonly List<Slot> lootSlots = new List<Slot>();
        private Slot selectedSlot;

        public Action<Slot> OnSlotAdded { get; set; }
        public Action<Slot> OnSlotRemoved { get; set; }

        private CellEntity playerEntity;
        private IMovementController playerMovement;
        private PlayerMoveController playerMoveController;

        private void Awake()
        {
            GameController.Instance.OnStageStart += StartStage;
            GameController.Instance.OnStageFinish += FinishStage;
        }

        private void StartStage(Stage stage)
        {
            playerEntity = stage.PlayerEntity;

            playerMoveController = playerEntity.GetComponent<PlayerMoveController>();

            playerMovement = playerEntity.GetComponent<IMovementController>();
            playerMovement.OnMovement += RefreshLootInventory;

            itemController.OnItemDrop += RefreshLootInventory;
            itemController.OnItemPickup += RefreshLootInventory;

            RefreshInventory(playerEntity.Cell);
        }

        private void FinishStage(Stage stage)
        {
            playerEntity.GetComponent<IMovementController>().OnMovement -= RefreshLootInventory;
            itemController.OnItemDrop -= RefreshLootInventory;
            itemController.OnItemPickup -= RefreshLootInventory;
            playerEntity = null;
        }

        private void CloseLootInventory()
        {
            foreach (Slot slot in new List<Slot>(lootSlots))
                DeleteSlot(slot);

            lootSlots.Clear();
            lootPanel.Clear();
        }

        private void RefreshLootInventory(IMovementController movementController, Cell from, Cell to)
        {
            CloseLootInventory();
            RefreshInventory(to);
        }

        private void RefreshLootInventory(LootItem lootItem, Cell cell)
        {
            if (cell == playerEntity.Cell)
                RefreshInventory(cell);
        }

        private void RefreshInventory(Cell cell)
        {
            List<LootItem> lootItems = itemController.GetItemEntities(cell);

            foreach (LootItem lootItem in lootItems)
            {
                if (!lootSlots.Find(s => s.LootItem == lootItem))
                {
                    Slot slot = CreateSlot(lootPanel, slotPrefab);
                    AttachSlotItem(slot, lootItem);
                    lootSlots.Add(slot);
                }
            }

            foreach (Slot slot in new List<Slot>(lootSlots))
            {
                if (slot.LootItem == null || !lootItems.Contains(slot.LootItem))
                {
                    DeleteSlot(slot);
                }
            }

            foreach (Slot slot in new List<Slot>(inventorySlots))
            {
                if (slot.LootItem == null)
                {
                    DeleteSlot(slot);
                }
            }
        }

        private Slot CreateSlot(ButtonPanel panel, GameObject slotPrefab)
        {
            Slot slot = panel.AddElement(slotPrefab).GetComponent<Slot>();
            OnSlotAdded?.Invoke(slot);
            return slot;
        }

        private void DeleteSlot(Slot slot)
        {
            OnSlotRemoved?.Invoke(slot);

            if (slot.LootItem != null)
            {
                slot.LootItem.OnItemDestroy -= DeleteItem;
            }

            if (inventoryPanel.Contains(slot.gameObject))
            {
                inventorySlots.Remove(slot);
                inventoryPanel.RemoveElement(slot.gameObject);
            }
            else if (lootPanel.Contains(slot.gameObject))
            {
                lootSlots.Remove(slot);
                lootPanel.RemoveElement(slot.gameObject);
            }
        }

        private void DeleteItem(LootItem lootItem)
        {
            Slot slot = lootSlots.Find(s => s.LootItem == lootItem);
            if (slot == null)
                slot = inventorySlots.Find(s => s.LootItem == lootItem);

            DeleteSlot(slot);
        }

        private void DeselectOnUse(EntityAction entityAction)
        {
            DeselectSlot();
        }

        public void DeselectSlot()
        {
            if (selectedSlot != null)
            {
                if (selectedSlot.LootItem != null)
                {
                    EntityAction entityAction = selectedSlot.LootItem.GetComponent<EntityAction>();
                    if (entityAction != null)
                        entityAction.OnMoveStart -= DeselectOnUse;
                }

                playerMoveController.DeselectAction();
            }

            selectedSlot = null;
            RefreshInventory(playerEntity.Cell);
        }

        private void SelectSlot(Slot slot)
        {
            selectedSlot = slot;

            EntityAction entityAction = slot.LootItem.GetComponent<EntityAction>();
            if (entityAction != null)
            {
                entityAction.OnMoveStart += DeselectOnUse;
                playerMoveController.SelectAction(entityAction);
            }

            if (lootSlots.Contains(slot))
            {
                if (inventorySlots.Count < maxInventorySize)
                {
                    inventorySlots.Add(CreateSlot(inventoryPanel, slotPrefab));
                }
            }
            else if (inventorySlots.Contains(slot))
            {
                lootSlots.Add(CreateSlot(lootPanel, slotPrefab));
            }
        }

        public void DragSlot(Slot slot)
        {
            if (selectedSlot != slot)
                PressSlot(slot);
        }

        public void PressSlot(Slot slot)
        {
            if (selectedSlot == null)
            {
                if (slot.LootItem != null)
                    SelectSlot(slot);
            }
            else if (selectedSlot == slot)
            {
                DeselectSlot();
            }
            else
            {
                SwapItems(selectedSlot, slot);
            }
        }

        public void SwapItems(Slot from, Slot to)
        {
            itemController.OnItemDrop -= RefreshLootInventory;
            itemController.OnItemPickup -= RefreshLootInventory;

            LootItem fromItem = from.LootItem;
            LootItem toItem = to.LootItem;

            if (fromItem == null)
                return;

            DetachSlotItem(from);
            DetachSlotItem(to);

            AttachSlotItem(to, fromItem);
            if (toItem != null)
                AttachSlotItem(from, toItem);

            DeselectSlot();

            itemController.OnItemDrop += RefreshLootInventory;
            itemController.OnItemPickup += RefreshLootInventory;
        }

        public bool DropOnCell(Cell cell)
        {
            if (playerMoveController.SelectedAction.CanInteract(cell))
            {
                playerMoveController.CheckMove(cell);
                return true;
            }
            return false;
        }

        private void AttachSlotItem(Slot slot, LootItem lootItem)
        {
            if (lootSlots.Contains(slot))
            {
                itemController.DropItem(lootItem, playerEntity.Cell);
            }

            slot.AttachItem(lootItem);
            lootItem.OnItemDestroy += DeleteItem;
        }

        private void DetachSlotItem(Slot slot)
        {
            if (slot.LootItem != null)
            {
                LootItem lootItem = slot.LootItem;

                slot.LootItem.OnItemDestroy -= DeleteItem;
                slot.DetachItem();

                if (lootSlots.Contains(slot))
                {
                    itemController.PickupItem(lootItem, playerEntity.Cell, playerEntity);
                }
            }
        }

        public void PickupItem(LootItem lootItem)
        {
            Slot newSlot = CreateSlot(inventoryPanel, slotPrefab);
            inventorySlots.Add(newSlot);
            newSlot.AttachItem(lootItem);
            lootItem.OnItemDestroy += DeleteItem;
            
            itemController.PickupItem(lootItem, playerEntity.Cell, playerEntity);
        }
    }
}
