using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    public void Connect(Tower tower)
    {
        int width = tower.Size.x;
        foreach (Room room in tower.Rooms)
            foreach (Room adjacentRoom in room.AdjacentRooms)
                if (!room.ConnectedRooms.Contains(adjacentRoom))
                    ConnectRooms(room, adjacentRoom, width);
    }

    private void ConnectRooms(Room first, Room second, int width)
    {
        if (first.ConnectedRooms.Count > 1)
        {
            int minLevel = Mathf.Min(first.LowestLevel, second.LowestLevel);
            int maxLevel = Mathf.Max(first.HighestLevel, second.HighestLevel);
            int[] cellCounts = new int[maxLevel - minLevel + 1];
            bool[,] roomHasCell = new bool[maxLevel - minLevel + 1, 2];

            first.Cells.ForEach(cell => { cellCounts[cell.Position.y - minLevel]++; roomHasCell[cell.Position.y - minLevel, 0] = true; });
            second.Cells.ForEach(cell => { cellCounts[cell.Position.y - minLevel]++; roomHasCell[cell.Position.y - minLevel, 1] = true; });
            for (int i = 0; i < cellCounts.Length; i++)
            {
                if (cellCounts[i] >= width)
                {
                    if (roomHasCell[i, 0] && roomHasCell[i, 1])
                    {
                        Room highestRoom = first.HighestLevel > second.HighestLevel ? first : first.HighestLevel < second.HighestLevel ? second : null;
                        if (highestRoom)
                        {
                            Room otherRoom = ReferenceEquals(first, highestRoom) ? second : first;
                            if (GetHighestRow(otherRoom, width).All(cell => !cell || cell.AdjacentRooms.Contains(highestRoom)))
                                break;
                        }
                        Room lowestRoom = first.LowestLevel < second.LowestLevel ? first : first.LowestLevel > second.LowestLevel ? second : null;
                        if (lowestRoom)
                        {
                            Room otherRoom = ReferenceEquals(first, lowestRoom) ? second : first;
                            if (GetLowestRow(otherRoom, width).All(cell => !cell || cell.AdjacentRooms.Contains(lowestRoom)))
                                break;
                        }
                        return;
                    }
                    else
                        break;
                }
            }
        }

        List<Cell> adjacentCells = first.Cells.FindAll(cell => cell.AdjacentRooms.Contains(second));

        Cell firstCell = adjacentCells[Random.Range(0, adjacentCells.Count)];
        foreach (Direction direction in Direction.Values)
            if (!(firstCell.AdjacentCells[direction] is null) && ReferenceEquals(firstCell.AdjacentCells[direction].Room, second))
            {
                ConnectCells(firstCell, firstCell.AdjacentCells[direction], direction);
                break;
            }
    }

    private void ConnectCells(Cell first, Cell second, Direction direction)
    {
        first.ConnectedCells[direction] = second;
        second.ConnectedCells[direction.Opposite] = first;

        first.Room.ConnectedRooms.Add(second.Room);
        second.Room.ConnectedRooms.Add(first.Room);

        first.ConnectedRooms.Add(second.Room);
        second.ConnectedRooms.Add(first.Room);
    }

    private Cell[] GetLowestRow(Room room, int width)
    {
        Cell[] lowestCells = new Cell[width];
        foreach (Cell cell in room.Cells)
            if (MathHelper.InRange(cell.Position.x, width) && (!lowestCells[cell.Position.x] || lowestCells[cell.Position.x].Position.y > cell.Position.y))
                lowestCells[cell.Position.x] = cell;
        return lowestCells;
    }
    private Cell[] GetHighestRow(Room room, int width)
    {
        Cell[] highestCells = new Cell[width];
        foreach (Cell cell in room.Cells)
            if (MathHelper.InRange(cell.Position.x, width) && (!highestCells[cell.Position.x] || highestCells[cell.Position.x].Position.y < cell.Position.y))
                highestCells[cell.Position.x] = cell;
        return highestCells;
    }
}