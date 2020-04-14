using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    public void Connect(Tower tower)
    {
        foreach (Room room in tower.Rooms)
            foreach (Room adjacentRoom in room.AdjacentRooms)
                if (!room.ConnectedRooms.Contains(adjacentRoom))
                    ConnectRooms(room, adjacentRoom);
    }

    private void ConnectRooms(Room first, Room second)
    {
        List<Cell> adjacentCells = first.Cells.FindAll(cell => cell.AdjacentRooms.Contains(second));

        while (adjacentCells.Count > 0)
        {
            int index = Random.Range(0, adjacentCells.Count);
            Cell cell = adjacentCells[index];

            foreach (Direction direction in Direction.Straights)
                if (!(cell.AdjacentCells[direction] is null) && ReferenceEquals(cell.AdjacentCells[direction].Room, second))
                {
                    ConnectCells(cell, cell.AdjacentCells[direction], direction);
                    return;
                }

            adjacentCells.RemoveAt(index);
        }
    }

    private void ConnectDiagonals(Cell first, Cell second, Direction direction)
    {
        Cell clockwiseCell = first.AdjacentCells[direction.Clockwise];
        if (clockwiseCell != null && clockwiseCell.Room == second.Room)
        {
            first.ConnectedCells[direction.Clockwise] = clockwiseCell;
            clockwiseCell.ConnectedCells[direction.Clockwise.Opposite] = first;
        }

        Cell counterclockwise = first.AdjacentCells[direction.Counterclockwise];
        if (counterclockwise != null && counterclockwise.Room == second.Room)
        {
            first.ConnectedCells[direction.Counterclockwise] = counterclockwise;
            counterclockwise.ConnectedCells[direction.Counterclockwise.Opposite] = first;
        }
    }

    private void ConnectCells(Cell first, Cell second, Direction direction)
    {
        first.ConnectedCells[direction] = second;
        second.ConnectedCells[direction.Opposite] = first;

        //ConnectDiagonals(first, second, direction);
        //ConnectDiagonals(second, first, direction.Opposite);

        first.Room.ConnectedRooms.Add(second.Room);
        second.Room.ConnectedRooms.Add(first.Room);

        first.ConnectedRooms.Add(second.Room);
        second.ConnectedRooms.Add(first.Room);
    }

    private Cell[] GetLowestRow(Room room, int width)
    {
        Cell[] lowestCells = new Cell[width];
        foreach (Cell cell in room.Cells)
            if (MathHelper.InRange(cell.Position.x, width) && (lowestCells[cell.Position.x] == null || lowestCells[cell.Position.x].Position.y > cell.Position.y))
                lowestCells[cell.Position.x] = cell;
        return lowestCells;
    }
    private Cell[] GetHighestRow(Room room, int width)
    {
        Cell[] highestCells = new Cell[width];
        foreach (Cell cell in room.Cells)
            if (MathHelper.InRange(cell.Position.x, width) && (highestCells[cell.Position.x] == null || highestCells[cell.Position.x].Position.y < cell.Position.y))
                highestCells[cell.Position.x] = cell;
        return highestCells;
    }
}