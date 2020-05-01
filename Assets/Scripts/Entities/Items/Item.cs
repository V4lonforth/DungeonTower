using UnityEngine;

public abstract class Item : Entity
{
    public Sprite icon;
    public int value;

    public Slot Slot { get; protected set; }

    public static Item CreateInstance(Item itemPrefab, Cell cell)
    {
        Item item = (Item)CreateInstance(itemPrefab.prefab, cell);
        cell.Items.Add(item);
        return item;
    }

    public static Item CreateInstance(Item itemPrefab, Slot slot)
    {
        Item item = Instantiate(itemPrefab.prefab).GetComponent<Item>();
        item.Slot = slot;
        slot.AttachItem(item);
        return item;
    }

    public void Attach(Cell cell)
    {
        Cell = cell;
        Cell.Entity = this;
        transform.position = Cell.WorldPosition;
    }

    public void Attach(Slot slot)
    {
        Slot = slot;
    }

    public void DetachFromCell()
    {
        if (Cell != null)
        {
            Cell.Items.Remove(this);
            Cell = null;
        }
    }

    public void DetachFromSlot()
    {
        if (Slot != null)
        {
            Slot = null;
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        Cell?.Items.Remove(this);
        Slot?.DetachItem();
    }

    public abstract void Use(Player player);
}