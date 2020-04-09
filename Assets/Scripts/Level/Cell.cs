using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Creature Creature { get; set; }
    public List<Item> Items { get; set; }

    public Vector2Int Position { get; private set; }
    public Room Room { get; private set; }
    public Tower Tower => Room.Tower;

    public Cell[] ConnectedCells { get; private set; }
    public Cell[] AdjacentCells { get; private set; }

    public List<Room> AdjacentRooms { get; private set; }
    public List<Room> ConnectedRooms { get; private set; }

    public GameObject[] Walls { get; private set; }

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

        Items = new List<Item>();
        Walls = new GameObject[Direction.DirectionsAmount];
    }

    public Direction GetDirectionToCell(Cell cell)
    {
        foreach (Direction direction in Direction.Values)
            if (ReferenceEquals(AdjacentCells[direction], cell))
                return direction;
        return Direction.Bottom;
    }

    public void OpenDoor(Cell cell)
    {
        if (cell.Room != Room)
        {
            Direction direction = GetDirectionToCell(cell);
            if (Walls[direction] != null)
                Walls[direction].GetComponent<Door>().Open(direction);
            else if (Walls[direction.Clockwise] != null && ConnectedCells[direction.Clockwise] != null)
                Walls[direction.Clockwise].GetComponent<Door>().Open(direction.Clockwise);
            else if (Walls[direction.Counterclockwise] != null && ConnectedCells[direction.Counterclockwise] != null)
                Walls[direction.Counterclockwise].GetComponent<Door>().Open(direction.Counterclockwise);
            else if (cell.Walls[direction.Opposite.Clockwise] != null && cell.ConnectedCells[direction.Opposite.Clockwise] != null)
                cell.Walls[direction.Opposite.Clockwise].GetComponent<Door>().Open(direction.Clockwise);
            else if (cell.Walls[direction.Opposite.Counterclockwise] != null && cell.ConnectedCells[direction.Opposite.Counterclockwise] != null)
                cell.Walls[direction.Opposite.Counterclockwise].GetComponent<Door>().Open(direction.Counterclockwise);
        }
    }
}