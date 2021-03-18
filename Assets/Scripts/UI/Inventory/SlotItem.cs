using DungeonTower.Entity.Items;
using DungeonTower.Input;
using DungeonTower.Inventory;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonTower.UI.Inventory
{
    public class SlotItem : MonoBehaviour
    {
        private TouchHandler touchHandler;
        private Slot slot;

        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image image;

        [SerializeField] private RectTransform parentTransform;

        private Vector2 startTouchPosition;
        private bool isDragging;

        public Image Image => image;
        public Action<Slot> OnSelect { get; set; }
        public Action<Slot> OnDrag { get; set; }
        public Action<Slot, Vector2> OnDrop { get; set; }

        private const float MinDistanceToDrag = 10f;

        private void Awake()
        {
            touchHandler = new TouchHandler(layer: 100, checkHit: CheckHit, onPress: Press, onHold: Hold, onRelease: Release, usingWorldPosition: false);
            touchHandler.Enable();
        }

        private bool CheckHit(Vector2 position)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, position);
        }

        private void Press(Vector2 position)
        {
            startTouchPosition = position;
            isDragging = false;
        }

        private void Hold(Vector2 position)
        {
            if (isDragging)
            {
                rectTransform.position = position;
            }
            else if ((position - startTouchPosition).sqrMagnitude >= MinDistanceToDrag * MinDistanceToDrag && slot.LootItem != null)
            {
                isDragging = true;
                OnDrag?.Invoke(slot);
                rectTransform.SetParent(parentTransform.root);
            }
        }

        private void Release(Vector2 position)
        {
            if (isDragging)
            {
                rectTransform.SetParent(parentTransform);
                OnDrop?.Invoke(slot, position);
                ResetPosition();
            }
            else
            {
                OnSelect?.Invoke(slot);
            }
        }

        private void ResetPosition()
        {
            rectTransform.anchoredPosition = Vector2.zero;
        }

        public void AttachToSlot(Slot slot)
        {
            this.slot = slot;
        }
        public void DetachSlot()
        {
            slot = null;
        }

        public void AttachItem(LootItem lootItem)
        {
            image.sprite = lootItem.Icon;
        }
        public void DetachItem()
        {
            image.sprite = null;
        }

        private void OnDisable()
        {
            touchHandler.Disable();
        }
        private void OnEnable()
        {
            touchHandler.Enable();
        }
    }
}
