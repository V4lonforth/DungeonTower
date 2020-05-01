using System.Collections.Generic;
using UnityEngine;

public class Navigator
{
    private Tower tower;
    private int[,] distance;
    private bool[,] visited;

    public Navigator(Tower tower)
    {
        this.tower = tower;
        distance = new int[tower.Size.y, tower.Size.x];
        visited = new bool[tower.Size.y, tower.Size.x];
    }

    private void ClearMap()
    {
        for (int y = 0; y < tower.Size.y; y++)
            for (int x = 0; x < tower.Size.x; x++)
                visited[y, x] = false;
    }

    public void CreateMap(Cell startCell)
    {
        ClearMap();
        visited[startCell.Position.y, startCell.Position.x] = true;

        Queue<Cell> queue = new Queue<Cell>();
        foreach (Cell connectedCell in startCell.ConnectedCells)
            if (connectedCell != null)
                queue.Enqueue(connectedCell);

        while (queue.Count > 0)
        {
            Cell cell = queue.Dequeue();
            if (visited[cell.Position.y, cell.Position.x])
                continue;
            visited[cell.Position.y, cell.Position.x] = true;
            int minDistance = int.MaxValue;
            foreach (Cell connectedCell in cell.ConnectedCells)
            {
                if (connectedCell != null && !connectedCell.Destroyed && !connectedCell.HasObstacle)
                {
                    if (visited[connectedCell.Position.y, connectedCell.Position.x])
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

    public List<Direction> GetDirections(Cell cell)
    {
        List<Direction> directions = new List<Direction>();
        foreach (Direction direction in Direction.Values)
        {
            Cell connectedCell = cell.ConnectedCells[direction];
            if (connectedCell != null)
            {
                if ((distance[cell.Position.y, cell.Position.x] > distance[connectedCell.Position.y, connectedCell.Position.x]) ||
                        (distance[cell.Position.y, cell.Position.x] == distance[connectedCell.Position.y, connectedCell.Position.x] &&
                        CompareDistance(cell.Position, connectedCell.Position) > 0))
                    directions.Add(direction);
            }
        }
        directions.Sort((a, b) => CompareDistance(a.ShiftPosition(cell.Position), b.ShiftPosition(cell.Position)));
        return directions;
    }

    private int CompareDistance(Vector2Int first, Vector2Int second)
    {
        Vector2Int firstOffset = tower[first].Position - tower.Player.Cell.Position;
        Vector2Int secondOffset = tower[second].Position - tower.Player.Cell.Position;
        return firstOffset.sqrMagnitude.CompareTo(secondOffset.sqrMagnitude);
    }
}