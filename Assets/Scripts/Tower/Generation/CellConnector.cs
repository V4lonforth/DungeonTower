using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellConnector
{
    private TowerGenerator towerGenerator;

    private const int MaxRoomSize = 50;

    public CellConnector(TowerGenerator towerGenerator)
    {
        this.towerGenerator = towerGenerator;
    }

    public void ConnectCells()
    {
        foreach (Room room in towerGenerator.Tower.Rooms)
            ConnectCellsInRoom(room);

        foreach (Room room in towerGenerator.Tower.Rooms)
            ConnectRooms(room);

        CutPointsFinder cutPointsFinder = new CutPointsFinder(MaxRoomSize);
        foreach (Room room in towerGenerator.Tower.Rooms)
            room.NonCutPoints = cutPointsFinder.FindNonCutPoints(room);
    }

    private void ConnectCellsInRoom(Room room)
    {
        foreach (Cell cell in room.Cells)
        {
            foreach (Direction direction in Direction.Values)
            {
                Vector2Int shiftedPos = direction.ShiftPosition(cell.Position);
                if (MathHelper.InRange(shiftedPos, towerGenerator.size))
                {
                    Cell adjacentCell = towerGenerator.Tower[shiftedPos];
                    AdjoinCells(cell, adjacentCell, direction);

                    if (cell.Room == adjacentCell.Room)
                        ConnectCells(cell, adjacentCell, direction);
                    else if (Direction.Straights.Contains(direction) && !room.AdjacentRooms.Contains(adjacentCell.Room))
                    {
                        room.AdjacentRooms.Add(adjacentCell.Room);
                        adjacentCell.Room.AdjacentRooms.Add(room);
                    }
                }
            }
        }
    }

    private void ConnectRooms(Room room)
    {
        foreach (Room adjacentRoom in room.AdjacentRooms)
        {
            if (room.ConnectedRooms.Contains(adjacentRoom))
                continue;

            List<Cell> borderCells = FindBorderCells(room, adjacentRoom);
            Cell borderCell = MathHelper.GetRandomElement(borderCells);

            foreach (Direction direction in Direction.Straights)
            {
                Cell adjacentCell = borderCell.AdjacentCells[direction];
                if (adjacentCell != null && adjacentCell.Room == adjacentRoom)
                {
                    ConnectCellsBetweenRooms(borderCell, adjacentCell, direction);
                    room.ConnectedRooms.Add(adjacentCell.Room);
                    adjacentCell.Room.ConnectedRooms.Add(room);
                    break;
                }
            }
        }
    }

    private void AdjoinCells(Cell first, Cell second, Direction direction)
    {
        first.AdjacentCells[direction] = second;
        second.AdjacentCells[direction.Opposite] = first;
    }
    private void ConnectCells(Cell first, Cell second, Direction direction)
    {
        first.ConnectedCells[direction] = second;
        second.ConnectedCells[direction.Opposite] = first;
    }

    private void ConnectCellsBetweenRooms(Cell first, Cell second, Direction direction)
    {
        ConnectCells(first, second, direction);

        first.Room.DoorCells.Add(first);
        second.Room.DoorCells.Add(second);

        //ConnectDiagonals(first, second, direction);
        //ConnectDiagonals(second, first, direction.Opposite);
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

    private List<Cell> FindBorderCells(Room first, Room second)
    {
        List<Cell> borderCells = new List<Cell>();
        foreach (Cell cell in first.Cells)
            foreach (Direction direction in Direction.Straights)
                if (cell.AdjacentCells[direction] != null && cell.AdjacentCells[direction].Room == second)
                    borderCells.Add(cell);
        return borderCells;
    }
}