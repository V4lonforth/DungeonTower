using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public Vector2Int size;
    public int maxRoomSize;

    public GameObject cellPrefab;
    public GameObject roomPrefab;
    public GameObject towerPrefab;

    public Tower Build()
    {
        Tower tower = Instantiate(towerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Tower>();

        Cell[,] cells = new Cell[size.y, size.x];
        List<Room> roomList = new List<Room>();

        for (Vector2Int pos = Vector2Int.zero; pos.x < size.x; pos.x++)
            for (pos.y = 0; pos.y < size.y; pos.y++)
                if (cells[pos.y, pos.x] is null)
                    roomList.Add(CreateRoom(cells, tower, pos));

        tower.Cells = cells;
        tower.Rooms = roomList.ToArray();
        tower.Size = size;
        tower.Navigator = new Navigator(tower);

        return tower;
    }

    private Room CreateRoom(Cell[,] cells, Tower tower, Vector2Int position)
    {
        Room room = Room.Instantiate(roomPrefab, tower);
        CreateCell(cells, room, position, 0, maxRoomSize);
        return room;
    }

    private float GetChance(int cellsGenerated, int maxCellsCount, int absentCellCount)
    {
        return (1 - (float)cellsGenerated / maxCellsCount) * (Direction.DirectionsAmount - absentCellCount) / 4f;
    }

    private List<Direction> FindAbsentCellDirections(Cell[,] cells, Vector2Int position)
    {
        List<Direction> directions = new List<Direction>();
        foreach (Direction direction in Direction.Straights)
        {
            Vector2Int shiftedPosition = direction.ShiftPosition(position);
            if (MathHelper.InRange(shiftedPosition, size) && cells[shiftedPosition.y, shiftedPosition.x] is null)
                directions.Add(direction);
        }
        return directions;
    }

    private int CreateCell(Cell[,] cells, Room room, Vector2Int position, int cellsGenerated, int maxCellsCount)
    {
        List<Direction> absentCellDirections = FindAbsentCellDirections(cells, position);
        float rand = Random.Range(0f, 1f);
        if (rand <= GetChance(cellsGenerated, maxCellsCount, absentCellDirections.Count))
        {
            cells[position.y, position.x] = Cell.Instantiate(cellPrefab, room, position);
            int localCellsGenerated = 1;
            foreach (Direction direction in absentCellDirections)
            {
                Vector2Int shiftedPosition = direction.ShiftPosition(position);
                if (MathHelper.InRange(shiftedPosition, size) && cells[shiftedPosition.y, shiftedPosition.x] == null)
                    localCellsGenerated += CreateCell(cells, room, shiftedPosition, cellsGenerated + localCellsGenerated, maxCellsCount);
            }
            return localCellsGenerated;
        }
        return 0;
    }
}