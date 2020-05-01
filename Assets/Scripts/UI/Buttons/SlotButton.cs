using UnityEngine;

public class SlotButton : Button
{
    public Vector2 StartTouchPosition { get; private set; }
    public Vector2 SlotPosition { get; private set; }

    private float holdingTime;
    private bool isDragging;

    private Slot slot;

    private const float MinDragDistance = 60f;
    private const float MinInspectTime = 0.5f;

    protected new void Awake()
    {
        base.Awake();
        onPress += Press;
        onHold += Hold;
        onRelease += Release;

        slot = GetComponent<Slot>();
        SlotPosition = area.position;
    }

    private void Press(Vector2 position)
    {
        holdingTime = 0f;
        isDragging = false;
        StartTouchPosition = position;
    }

    private void Hold(Vector2 position)
    {
        if (!isDragging)
        {
            if (Vector2.Distance(position, StartTouchPosition) > MinDragDistance)
                isDragging = true;
        }
        if (isDragging)
            area.position = position;
        else
            holdingTime += Time.deltaTime;
    }

    private void Release(Vector2 position)
    {
        area.position = SlotPosition;
        if (isDragging)
        {
            Slot draggedSlot = slot.inventory.FindDropEquipmentSlot(position);
            if (draggedSlot == null)
                slot.inventory.DropItem(slot);
            else
                slot.inventory.SwapItems(slot, draggedSlot);
        }
        else if (holdingTime < MinInspectTime)
        {
            slot.inventory.Use(slot);
        }
    }
}