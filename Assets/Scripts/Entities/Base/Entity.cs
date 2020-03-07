using System.Collections;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Cell Cell { get; protected set; }
    public Room Room => Cell.Room;
    public Tower Tower => Room.Tower;
    public TurnController TurnController => Tower.TurnController;

    public float movingTime;

    public static Entity Instantiate(GameObject prefab, Cell cell)
    {
        Entity entity = Instantiate(prefab, cell.transform).GetComponent<Entity>();
        entity.Cell = cell;
        cell.Entity = entity;
        return entity;
    }
    
    private IEnumerator MoveToParentCell(float movingTimeLeft)
    {
        while (movingTimeLeft > 0f)
        {
            if (Time.deltaTime >= movingTimeLeft)
            {
                movingTimeLeft = 0f;
                StopMoving();
            }
            else
            {
                transform.position += (Cell.transform.position - transform.position) * (Time.deltaTime / movingTimeLeft);
                movingTimeLeft -= Time.deltaTime;
                yield return null;
            }
        }
    }

    protected virtual void StopMoving()
    {
        transform.position = Cell.transform.position;
    }

    public virtual void MoveTo(Cell cell)
    {
        if (Cell != null && Cell.Entity == this)
            Cell.Entity = null;
        cell.Entity = this;
        Cell = cell;

        StartCoroutine(MoveToParentCell(movingTime));
    }

    public virtual void Replace(Cell cell)
    {
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
            MoveTo(cell);
    }

    public virtual void Destroy()
    {
        Destroy(gameObject);
        Cell.Entity = null;
    }
}