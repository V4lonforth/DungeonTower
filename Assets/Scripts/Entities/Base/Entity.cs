using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Cell Cell { get; protected set; }
    public Room Room => Cell.Room;
    public Tower Tower => Room.Tower;
    public TurnController TurnController => Tower.TurnController;

    public static Entity Instantiate(GameObject prefab, Cell cell)
    {
        Entity entity = Instantiate(prefab, cell.transform).GetComponent<Entity>();
        entity.Cell = cell;
        return entity;
    }

    public virtual void Destroy()
    {
        Destroy(gameObject);
    }

    public abstract string GetDescription();
}