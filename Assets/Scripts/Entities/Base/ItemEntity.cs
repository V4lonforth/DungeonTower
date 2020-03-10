using UnityEngine;

public class ItemEntity : Entity
{
    public Item Item { get; private set; }
    public EquipmentSlot EquipmentSlot { get; private set; }

    public new static ItemEntity Instantiate(GameObject prefab, Cell cell)
    {
        ItemEntity entity = (ItemEntity)Entity.Instantiate(prefab, cell);
        cell.ItemEntities.Add(entity);
        return entity;
    }

    private void Awake()
    {
        Item = GetComponent<Item>();
        Item.ItemEntity = this;
    }

    public override void Destroy()
    {
        base.Destroy();
        Cell?.ItemEntities.Remove(this);
        EquipmentSlot?.DetachItem();
    }

    public void DetachFromCell()
    {
        if (Cell != null)
        {
            Cell.ItemEntities.Remove(this);
            Cell = null;
        }
    }

    public void DetachFromEquipmentSlot()
    {
        if (EquipmentSlot != null)
        {
            EquipmentSlot.DetachItem();
            EquipmentSlot = null;
        }
    }

    public void AttachToEquipmentSlot(EquipmentSlot equipmentSlot)
    {
        EquipmentSlot = equipmentSlot;
    }

    public void AttachToCell(Cell cell)
    {
        cell.ItemEntities.Add(this);
        Cell = cell;
        transform.SetParent(cell.transform);
        transform.position = cell.transform.position;
    }

    public override string GetDescription()
    {
        return Item.GetDescription();
    }
}