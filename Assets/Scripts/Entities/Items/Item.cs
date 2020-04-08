using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Cell Cell { get; private set; }
    public EquipmentSlot EquipmentSlot { get; private set; }

    public GameObject prefab;
    public Sprite icon;
    public int value;
    public bool collectable;

    public static Item Instantiate(Item item, Cell cell)
    {
        Item entity = Instantiate(item.prefab, cell.transform).GetComponent<Item>();
        entity.Cell = cell;
        cell.ItemEntities.Add(entity);
        return entity;
    }

    public void Destroy()
    {
        Destroy(gameObject);
        Cell?.ItemEntities.Remove(this);
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
        cell.ItemEntities.Add(this);
        Cell = cell;
        transform.SetParent(cell.transform);
        transform.position = cell.transform.position;
    }

    public void DetachFromCell()
    {
        if (Cell != null)
        {
            Cell.ItemEntities.Remove(this);
            Cell = null;
        }
    }

    public abstract void Use(PlayerEntity player);
    public abstract string GetDescription();
}