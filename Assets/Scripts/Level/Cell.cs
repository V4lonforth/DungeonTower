using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public CreatureEntity CreatureEntity { get; set; }
    public List<ItemEntity> ItemEntities { get; set; }

    public Vector2Int Position { get; private set; }
    public Room Room { get; private set; }
    public Tower Tower => Room.Tower;

    public Cell[] ConnectedCells { get; private set; }
    public Cell[] AdjacentCells { get; private set; }

    public List<Room> AdjacentRooms { get; private set; }
    public List<Room> ConnectedRooms { get; private set; }

    public static Cell Instantiate(GameObject cellPrefab, Room room, Vector2Int position)
    {
        Cell cell = Instantiate(cellPrefab, room.transform).GetComponent<Cell>();
        cell.Room = room;
        cell.Position = position;
        cell.transform.position = new Vector3(position.x, position.y);
        room.AddCell(cell);
        return cell;
    }

    private void Awake()
    {
        ConnectedCells = new Cell[Direction.DirectionsAmount];
        AdjacentCells = new Cell[Direction.DirectionsAmount];

        AdjacentRooms = new List<Room>();
        ConnectedRooms = new List<Room>();

        ItemEntities = new List<ItemEntity>();
    }

    public Direction GetDirectionToCell(Cell cell)
    {
        foreach (Direction direction in Direction.Values)
            if (ReferenceEquals(AdjacentCells[direction], cell))
                return direction;
        return Direction.Bottom;
    }
}