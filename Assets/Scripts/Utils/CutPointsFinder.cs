using System.Collections.Generic;
using UnityEngine;

public class CutPointsFinder
{
    private bool[] visited;
    private int[] tin;
    private int[] fup;
    private int time;

    private Room currentRoom;
    private List<Cell> currentNonCutPoints;
    private Dictionary<Cell, int> currentIndices;

    public CutPointsFinder(int size)
    {
        visited = new bool[size];
        tin = new int[size];
        fup = new int[size];
    }

    public List<Cell> FindNonCutPoints(Room room)
    {
        currentNonCutPoints = new List<Cell>();
        currentRoom = room;
        currentIndices = new Dictionary<Cell, int>(currentRoom.Cells.Count);
        for (int i = 0; i < room.Cells.Count; i++)
            currentIndices.Add(room.Cells[i], i);

        for (int i = 0; i < visited.Length; i++)
            visited[i] = false;
        time = 0;
        Dfs(0);
        return currentNonCutPoints;
    }

    private void Dfs(int v, int parent = -1)
    {
        time++;
        tin[v] = fup[v] = time;
        visited[v] = true;
        int count = 0;
        Cell cell = currentRoom.Cells[v];
        foreach (Direction direction in Direction.Straights)
        {
            Cell connectedCell = cell.ConnectedCells[direction];
            if (connectedCell != null)
            {
                currentIndices.TryGetValue(connectedCell, out int to);
                if (to != parent && connectedCell.Room == currentRoom)
                {
                    if (visited[to])
                        fup[v] = Mathf.Min(fup[v], tin[to]);
                    else
                    {
                        Dfs(to, v);
                        count++;
                        fup[v] = Mathf.Min(fup[v], fup[to]);
                        if (fup[to] < tin[v] && parent != -1)
                            currentNonCutPoints.Add(currentRoom.Cells[v]);
                    }
                }
            }
        }
        if (parent == -1 && count < 2)
            currentNonCutPoints.Add(currentRoom.Cells[v]);
    }
}