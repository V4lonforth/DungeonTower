using UnityEngine;

public abstract class Item : Entity
{
    public EquipmentSlot EquipmentSlot { get; private set; }

    public GameObject prefab;
    public Sprite icon;
    public int value;
    public bool collectable;

    public static Item Instantiate(Item itemPrefab, Cell cell)
    {
        Item item = Instantiate(itemPrefab.prefab, cell).GetComponent<Item>();
        cell.Items.Add(item);
        return item;
    }

    public override void Destroy()
    {
        base.Destroy();
        Cell?.Items.Remove(this);
        EquipmentSlot?.DetachItem();
    }

    public void AttachToEquipmentSlot(EquipmentSlot equipmentSlot)
    {
        EquipmentSlot = equipmentSlot;
    }

    public void DetachFromEquipmentSlot()
    {
        if (EquipmentSlot != null)
        {
            EquipmentSlot.DetachItem();
            EquipmentSlot = null;
        }
    }

    public void AttachToCell(Cell cell)
    {
        cell.Items.Add(this);
        Cell = cell;
        transform.position = cell.WorldPosition;
    }

    public void DetachFromCell()
    {
        if (Cell != null)
        {
            Cell.Items.Remove(this);
            Cell = null;
        }
    }

    public abstract void Use(Player player);
}