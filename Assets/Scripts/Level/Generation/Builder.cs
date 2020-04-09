using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public Vector2Int size;
    public int maxRoomSize;
    public int minRoomSize;

    public GameObject cellPrefab;
    public GameObject roomPrefab;
    public GameObject towerPrefab;

    public Tower Build()
    {
        Tower tower = Instantiate(towerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Tower>();

        Cell[,] cells = new Cell[size.y, size.x];
        List<Room> roomList = new List<Room>();
        tower.Cells = cells;
        tower.Size = size;

        for (Vector2Int pos = Vector2Int.zero; pos.y < size.y; pos.y++)
            for (pos.x = 0; pos.x < size.x; pos.x++)
                if (cells[pos.y, pos.x] is null)
                    roomList.Add(CreateRoom(tower, pos, Random.Range(minRoomSize, maxRoomSize + 1)));

        tower.Rooms = roomList.ToArray();
        tower.Navigator = new Navigator(tower);

        return tower;
    }

    private Room CreateRoom(Tower tower, Vector2Int position, int maxSize)
    {
        Room room = Room.Instantiate(roomPrefab, tower);
        List<Vector2Int> surroundings = new List<Vector2Int>();
        AddCell(tower, room, position, surroundings);
        int currentSize = 1;
        while (currentSize < maxSize && surroundings.Count > 0)
        {
            SortSurroundings(surroundings, position);
            int index = 0;
            Vector2Int newPosition = surroundings[index];
            surroundings.RemoveAt(index);
            AddCell(tower, room, newPosition, surroundings);
            currentSize++;
        }
        return room;
    }

    private void SortSurroundings(List<Vector2Int> surroundings, Vector2Int center)
    {
        surroundings.Sort((a, b) => Mathf.Max(a.x - center.x, a.y - center.y).CompareTo(Mathf.Max(b.x - center.x, b.y - center.y)));
    }

    private void AddCell(Tower tower, Room room, Vector2Int position, List<Vector2Int> surroundings)
    {
        tower[position] = Cell.Instantiate(cellPrefab, room, position);
        foreach (Direction direction in Direction.Values)
        {
            Vector2Int shiftedPosition = direction.ShiftPosition(position);
            if (MathHelper.InRange(shiftedPosition, tower.Size) && tower[shiftedPosition] == null && !surroundings.Contains(shiftedPosition))
                surroundings.Add(shiftedPosition);
        }
    }
}