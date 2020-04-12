using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Cell Cell { get; protected set; }
    public Room Room => Cell.Room;
    public Tower Tower => Room.Tower;

    public static Entity Instantiate(GameObject prefab, Cell cell)
    {
        Entity entity = Instantiate(prefab).GetComponent<Entity>();
        entity.Cell = cell;
        entity.transform.position = cell.WorldPosition;
        return entity;
    }

    public virtual void Destroy()
    {
        Destroy(gameObject);
    }
    public abstract string GetDescription();
}