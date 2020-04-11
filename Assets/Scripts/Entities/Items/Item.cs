using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Cell Cell { get; private set; }
    public EquipmentSlot EquipmentSlot { get; private set; }

    public GameObject prefab;
    public Sprite icon;
    public int value;
    public bool collectable;

    public static Item Instantiate(Item itemPrefab, Cell cell)
    {
        Item item = Instantiate(itemPrefab.prefab).GetComponent<Item>();
        item.transform.position = cell.WorldPosition;
        item.Cell = cell;
        cell.Items.Add(item);
        return item;
    }

    public void Destroy()
    {
        Destroy(gameObject);
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
        //transform.SetParent(cell.transform);
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
    public abstract string GetDescription();
}