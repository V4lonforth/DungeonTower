using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public Vector2Int size;
    public int maxRoomSize;
    public int minRoomSize;

    public float squareness;

    public Tower Build(TowerGenerator towerGenerator)
    {
        Tower tower = new Tower(size, towerGenerator, FindObjectOfType<Favor>());

        for (Vector2Int pos = Vector2Int.zero; pos.x < size.x; pos.x++)
            for (pos.y = 0; pos.y < size.y; pos.y++)
                if (tower[pos] is null)
                    tower.Rooms.Add(CreateRoom(tower, pos, Random.Range(minRoomSize, maxRoomSize + 1)));

        return tower;
    }

    private Room CreateRoom(Tower tower, Vector2Int position, int maxSize)
    {
        Room room = new Room(tower);
        List<Vector2Int> surroundings = new List<Vector2Int>();
        AddCell(tower, room, position, surroundings);
        int currentSize = 1;
        while (currentSize < maxSize && surroundings.Count > 0)
        {
            SortSurroundings(surroundings, position);
            int index = GetIndex(surroundings.Count);
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

    private int GetIndex(int size)
    {
        float value = Mathf.Pow(Random.Range(0f, 1f), squareness);
        for (int i = 0; i < size; i++)
            if (value < (float)(i + 1) / size)
                return i;
        return size - 1;
    }

    private void AddCell(Tower tower, Room room, Vector2Int position, List<Vector2Int> surroundings)
    {
        tower[position] = new Cell(room, position);
        room.AddCell(tower[position]);
        foreach (Direction direction in Direction.Straights)
        {
            Vector2Int shiftedPosition = direction.ShiftPosition(position);
            if (MathHelper.InRange(shiftedPosition, tower.Size) && tower[shiftedPosition] == null && !surroundings.Contains(shiftedPosition))
                surroundings.Add(shiftedPosition);
        }
    }
}