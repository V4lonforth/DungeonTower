using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Cell Cell { get; protected set; }
    public Room Room => Cell.Room;
    public Tower Tower => Room.Tower;

    public static Entity Instantiate(GameObject prefab, Cell cell)
    {
        Entity entity = Instantiate(prefab, cell.transform).GetComponent<Entity>();
        entity.Cell = cell;
        cell.Entity = entity;
        return entity;
    }

    public virtual void MoveTo(Cell cell)
    {
        cell.Entity = this;
        Cell = cell;
        transform.position = cell.transform.position;
    }

    public virtual void Replace(Cell cell)
    {
        Cell.Entity = null;
        cell.Entity?.Destroy();
        MoveTo(cell);
    }

    public virtual void Swap(Cell cell)
    {
        if (cell.Entity)
        {
            cell.Entity.MoveTo(Cell);
            MoveTo(cell);
        }
        else
            Replace(cell);
    }

    public virtual void Destroy()
    {
        Destroy(gameObject);
        Cell.Entity = null;
    }
}