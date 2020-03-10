using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IInteractive
{
    public Sprite emptyIcon;

    public Image equipmentIcon;
    public Inventory inventory;

    public Vector2 StartTouchPosition { get; private set; }
    public Vector2 SlotPosition { get; private set; }
    public Item Item { get; private set; }

    public bool Active { get; private set; }

    private bool pressed;
    private int touchId;

    private float holdingTime;
    private bool isDragging;

    private const float MinDragDistance = 60f;
    private const float MinInspectTime = 0.5f;

    private void Awake()
    {
        Active = true;
        SlotPosition = equipmentIcon.rectTransform.position;
    }

    public void Show(Item item)
    {
        Item = item;
        item.ItemEntity?.AttachToEquipmentSlot(this);
        equipmentIcon.sprite = item.icon;
        SlotPosition = equipmentIcon.rectTransform.position;
        gameObject.SetActive(true);
        Active = true;
    }

    public void Hide()
    {
        DetachItem();
        gameObject.SetActive(false);
        Active = false;
    }

    public void DetachItem()
    {
        Item = null;
        equipmentIcon.sprite = emptyIcon;
    }

    public bool Press(Vector2 position, int id)
    {
        if (!pressed && CheckDropPosition(position))
        {
            pressed = true;
            touchId = id;
            holdingTime = 0f;
            isDragging = false;
            StartTouchPosition = position;
            return true;
        }
        return false;
    }

    public bool Hold(Vector2 position, int id)
    {
        if (pressed && touchId == id)
        {
            if (!isDragging)
            {
                if (Vector2.Distance(position, StartTouchPosition) > MinDragDistance)
                    isDragging = true;
            }
            if (isDragging)
            {
                equipmentIcon.rectTransform.position = position;
            }
            else
            {
                holdingTime += Time.deltaTime;
            }
            return true;
        }
        return false;
    }

    public bool Release(Vector2 position, int id)
    {
        if (pressed && touchId == id)
        {
            pressed = false;
            equipmentIcon.rectTransform.position = SlotPosition;
            if (isDragging)
            {
                EquipmentSlot draggedSlot = inventory.FindDropEquipmentSlot(position);
                if (draggedSlot == null)
                    inventory.TryDropEquipment(this);
                else
                    inventory.TryDragEquipment(this, draggedSlot);
            }
            else if (holdingTime < MinInspectTime)
            {
                inventory.Use(this);
            }
            return true;
        }
        return false;
    }

    public bool CheckDropPosition(Vector2 position)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(equipmentIcon.rectTransform, position);
        //return Mathf.Abs(position.x - SlotPosition.x) * 2f < equipmentIcon.rectTransform.sizeDelta.x && Mathf.Abs(position.y - SlotPosition.y) * 2f < equipmentIcon.rectTransform.sizeDelta.y;
    }
}