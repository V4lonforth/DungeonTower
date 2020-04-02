using UnityEngine;

public abstract class ItemEntity : Entity
{
    public int value;
    public bool collectable;
    public Sprite icon;

    public EquipmentSlot EquipmentSlot { get; private set; }

    public new static ItemEntity Instantiate(GameObject prefab, Cell cell)
    {
        ItemEntity entity = (ItemEntity)Entity.Instantiate(prefab, cell);
        cell.ItemEntities.Add(entity);
        return entity;
    }

    public override void Destroy()
    {
        base.Destroy();
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
}