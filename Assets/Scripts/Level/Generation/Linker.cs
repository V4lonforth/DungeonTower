using System.Linq;
using UnityEngine;

public class Linker : MonoBehaviour
{
    public void Link(Tower tower)
    {
        for (Vector2Int pos = Vector2Int.zero; pos.x < tower.Size.x; pos.x++)
            for (pos.y = 0; pos.y < tower.Size.y; pos.y++)
                foreach (Direction direction in Direction.Values)
                {
                    Vector2Int shiftedPos = direction.ShiftPosition(pos);
                    if (MathHelper.InRange(shiftedPos, tower.Size))
                    {
                        if (ReferenceEquals(tower[pos].Room, tower[shiftedPos].Room))
                            ConnectCells(tower[pos], tower[shiftedPos], direction);
                        else
                            AdjoinCells(tower[pos], tower[shiftedPos], direction);
                    }

                }
    }

    private void ConnectCells(Cell first, Cell second, Direction direction)
    {
        first.ConnectedCells[direction] = second;
        second.ConnectedCells[direction.Opposite] = first;

        first.AdjacentCells[direction] = second;
        second.AdjacentCells[direction.Opposite] = first;
    }

    private void AdjoinCells(Cell first, Cell second, Direction direction)
    {
        first.AdjacentCells[direction] = second;
        second.AdjacentCells[direction.Opposite] = first;

        first.AdjacentRooms.Add(second.Room);
        second.AdjacentRooms.Add(first.Room);

        if (Direction.Straights.Contains(direction))
        {
            if (!first.Room.AdjacentRooms.Contains(second.Room))
                first.Room.AdjacentRooms.Add(second.Room);
            if (!second.Room.AdjacentRooms.Contains(first.Room))
                second.Room.AdjacentRooms.Add(first.Room);
        }
    }

    public void UnlinkCell(Cell cell)
    {
        foreach (Direction direction in Direction.Values)
        {
            if (cell.AdjacentCells[direction] != null)
                cell.AdjacentCells[direction].AdjacentCells[direction.Opposite] = null;
            if (cell.ConnectedCells[direction] != null)
                cell.ConnectedCells[direction].ConnectedCells[direction.Opposite] = null;
        }
    }
}