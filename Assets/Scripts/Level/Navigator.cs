using System.Collections.Generic;
using UnityEngine;

public class Navigator
{
    private Tower tower;
    private int[,] distance;
    private bool[,] seen;

    public Navigator(Tower tower)
    {
        this.tower = tower;
        distance = new int[tower.Size.y, tower.Size.x];
        seen = new bool[tower.Size.y, tower.Size.x];
    }

    private void ClearMap()
    {
        for (int y = 0; y < tower.Size.y; y++)
            for (int x = 0; x < tower.Size.x; x++)
            {
                seen[y, x] = false;
                distance[y, x] = 0;
            }
    }

    public void CreateMap(Cell startCell)
    {
        ClearMap();

        seen[startCell.Position.y, startCell.Position.x] = true;
        Queue<Cell> queue = new Queue<Cell>();
        foreach (Direction direction in Direction.Values)
            if (startCell.ConnectedCells[direction])
                queue.Enqueue(startCell.ConnectedCells[direction]);

        while (queue.Count > 0)
        {
            Cell cell = queue.Dequeue();
            if (seen[cell.Position.y, cell.Position.x])
                continue;
            seen[cell.Position.y, cell.Position.x] = true;
            int minDistance = int.MaxValue;
            foreach (Cell connectedCell in cell.ConnectedCells)
            {
                if (connectedCell)
                {
                    if (seen[connectedCell.Position.y, connectedCell.Position.x])
                    {
                        if (minDistance > distance[connectedCell.Position.y, connectedCell.Position.x])
                            minDistance = distance[connectedCell.Position.y, connectedCell.Position.x];
                    }
                    else
                    {
                        queue.Enqueue(connectedCell);
                    }
                }
            }
            distance[cell.Position.y, cell.Position.x] = minDistance + 1;
        }
    }

    public int GetDistance(Cell cell)
    {
        return distance[cell.Position.y, cell.Position.x];
    }

    public Direction GetDirection(Cell cell)
    {
        foreach (Direction direction in Direction.Values)
        {
            Cell connectedCell = cell.ConnectedCells[direction];
            if (connectedCell && distance[cell.Position.y, cell.Position.x] > distance[connectedCell.Position.y, connectedCell.Position.x])
                return direction;
        }
        return Direction.Top;
    }

    public List<Direction> GetDirections(Cell cell)
    {
        List<Direction> directions = new List<Direction>();
        foreach (Direction direction in Direction.Values)
        {
            Cell connectedCell = cell.ConnectedCells[direction];
            if (connectedCell && distance[cell.Position.y, cell.Position.x] > distance[connectedCell.Position.y, connectedCell.Position.x])
                directions.Add(direction);
        }
        directions.Sort((a, b) =>
        {
            Vector2Int offsetA = a.ShiftPosition(cell.Position) - tower.Player.Cell.Position;
            Vector2Int offsetB = b.ShiftPosition(cell.Position) - tower.Player.Cell.Position;
            return offsetA.sqrMagnitude.CompareTo(offsetB.sqrMagnitude);
            //return (Mathf.Abs(offsetB.x) + Mathf.Abs(offsetB.y)).CompareTo(Mathf.Abs(offsetA.x) + Mathf.Abs(offsetA.y));
        });
        return directions;
    }
}