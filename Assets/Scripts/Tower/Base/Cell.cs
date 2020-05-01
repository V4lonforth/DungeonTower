using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell
{
    public Room Room { get; private set; }
    public Tower Tower => Room.Tower;

    public Vector2Int Position { get; private set; }
    public Vector3 WorldPosition => Tower.TowerGenerator.floorTilemap.CellToWorld((Vector3Int)Position);

    public Entity Entity { get; set; }
    public List<Item> Items { get; private set; }

    public Cell[] ConnectedCells { get; private set; }
    public Cell[] AdjacentCells { get; private set; }

    public bool HasObstacle => Entity is Obstacle;
    public bool Destroyed { get; set; }

    public Cell(Room room, Vector2Int position)
    {
        Room = room;
        Position = position;

        Items = new List<Item>();

        ConnectedCells = new Cell[Direction.DirectionsAmount];
        AdjacentCells = new Cell[Direction.DirectionsAmount];
    }

    public Direction GetDirectionToCell(Cell cell)
    {
        foreach (Direction direction in Direction.Values)
            if (AdjacentCells[direction] == cell)
                return direction;
        return Direction.BottomRight;
    }

    public bool IsConnected(Cell cell)
    {
        return ConnectedCells.Contains(cell);
    }
}