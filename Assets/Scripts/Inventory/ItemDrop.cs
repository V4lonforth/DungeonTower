using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public Slot[] dropSlots;

    public int dropOffset;
    public int dropsInterval;
    public RectTransform dropTransform;

    public List<Item> CurrentItems { get; private set; }
    public int CurrentOffset { get; private set; }

    public void Hide()
    {
        dropTransform.gameObject.SetActive(false);
        foreach (Slot slot in dropSlots)
            slot.RemoveItem();
    }

    public void Show(List<Item> items, int offset = 0)
    {
        Hide();
        CurrentItems = items;
        CurrentOffset = offset;
        if (items == null || items.Count == 0)
            return;

        dropTransform.anchoredPosition = new Vector2(dropOffset - dropsInterval * Mathf.Min(items.Count, dropSlots.Length), 0);
        dropTransform.gameObject.SetActive(true);

        for (int i = 0; i + offset < items.Count && i < dropSlots.Length; i++)
            dropSlots[i].SetItem(items[i + offset]);
    }
}