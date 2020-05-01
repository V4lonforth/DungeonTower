using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator
{
    private TowerGenerator towerGenerator;

    public RoomGenerator(TowerGenerator towerGenerator)
    {
        this.towerGenerator = towerGenerator;
    }

    public void GenerateRooms()
    {
        for (Vector2Int pos = Vector2Int.zero; pos.x < towerGenerator.size.x; pos.x++)
            for (pos.y = 0; pos.y < towerGenerator.size.y; pos.y++)
                if (towerGenerator.Tower[pos] is null)
                    towerGenerator.Tower.Rooms.Add(CreateRoom(towerGenerator.Tower, pos, MathHelper.GetRandomElement(towerGenerator.roomTypes)));
    }

    private Room CreateRoom(Tower tower, Vector2Int position, RoomType roomType)
    {
        Room room = new Room(tower, roomType);
        int maxSize = Random.Range(roomType.minSize, roomType.maxSize);

        List<Vector2Int> surroundings = new List<Vector2Int>();
        AddCell(tower, room, position, surroundings);
        int currentSize = 1;
        while (currentSize < maxSize && surroundings.Count > 0)
        {
            SortSurroundings(surroundings, position);
            int index = GetIndex(surroundings.Count, roomType.squareness);
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

    private int GetIndex(int size, float squareness)
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
        room.Cells.Add(tower[position]);
        foreach (Direction direction in Direction.Straights)
        {
            Vector2Int shiftedPosition = direction.ShiftPosition(position);
            if (MathHelper.InRange(shiftedPosition, tower.Size) && tower[shiftedPosition] == null && !surroundings.Contains(shiftedPosition))
                surroundings.Add(shiftedPosition);
        }
    }
}