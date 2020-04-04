using System;
using System.Collections.Generic;
using UnityEngine;

public class Decorator : MonoBehaviour
{
    public RoomDecorations defaultRoom;

    public GameObject backgroundPrefab;

    public GameObject horizontalWallPrefab;
    public GameObject verticalWallPrefab;

    public GameObject doorPrefab;
    public GameObject trapdoorPrefab;

    public GameObject platformPrefab;
    public GameObject lPlatformPrefab;
    public GameObject rPlatformPrefab;
    public GameObject lrPlatformPrefab;

    private Dictionary<int, GameObject> platforms;

    protected void Awake()
    {
        platforms = new Dictionary<int, GameObject>()
        {
            { 0b00, platformPrefab },
            { 0b01, rPlatformPrefab },
            { 0b10, lPlatformPrefab },
            { 0b11, lrPlatformPrefab },
        };
    }

    public void Decorate(Tower tower)
    {
        bool[,] decorated = new bool[tower.Size.y, tower.Size.x];

        foreach (Room room in tower.Rooms)
            foreach (Cell cell in room.Cells)
                DecorateCell(cell, decorated);

        for (Vector2Int pos = new Vector2Int(-1, -1); pos.y < tower.Size.y; pos.y++)
            for (pos.x = -1; pos.x < tower.Size.x; pos.x++)
                DecorateConnectors(tower, pos);
    }

    private void DecorateConnectors(Tower tower, Vector2Int pos)
    {
        bool[] walls = new bool[Direction.DirectionsAmount];
        Vector2Int localSize = new Vector2Int(2, 2);
        Cell[,] cells = new Cell[localSize.y, localSize.x];
        for (Vector2Int localPos = Vector2Int.zero; localPos.y < localSize.y; localPos.y++)
            for (localPos.x = 0; localPos.x < localSize.x; localPos.x++)
                cells[localPos.y, localPos.x] = MathHelper.InRange(pos + localPos, tower.Size) ? tower[pos + localPos] : null;

        Cell cell = null;
        if (cells[0, 0])
        {
            cell = cells[0, 0];
            if (!cells[0, 1] || !cells[0, 0].ConnectedCells[Direction.Right])
                walls[Direction.Bottom] = true;
            if (!cells[1, 0] || !cells[0, 0].ConnectedCells[Direction.Top])
                walls[Direction.Left] = true;

            if (cells[0, 1] && !ReferenceEquals(cells[0, 0].Room, cells[0, 1].Room))
                walls[Direction.Bottom] = true;

            if (cells[1, 0] && !ReferenceEquals(cells[1, 0].Room, cells[0, 0].Room))
                walls[Direction.Left] = true;
        }
        if (cells[0, 1])
        {
            cell = cells[0, 1];
            if (!cells[0, 0] || !cells[0, 1].ConnectedCells[Direction.Left])
                walls[Direction.Bottom] = true;
            if (!cells[1, 1] || !cells[0, 1].ConnectedCells[Direction.Top])
                walls[Direction.Right] = true;

            if (cells[0, 0] && !ReferenceEquals(cells[0, 0].Room, cells[0, 1].Room))
                walls[Direction.Bottom] = true;

            if (cells[1, 1] && !ReferenceEquals(cells[1, 1].Room, cells[0, 1].Room))
                walls[Direction.Right] = true;
        }
        if (cells[1, 0])
        {
            cell = cells[1, 0];
            if (!cells[1, 1] || !cells[1, 0].ConnectedCells[Direction.Right])
                walls[Direction.Top] = true;
            if (!cells[0, 0] || !cells[1, 0].ConnectedCells[Direction.Bottom])
                walls[Direction.Left] = true;

            if (cells[1, 1] && !ReferenceEquals(cells[1, 0].Room, cells[1, 1].Room))
                walls[Direction.Top] = true;

            if (cells[0, 0] && !ReferenceEquals(cells[0, 0].Room, cells[1, 0].Room))
                walls[Direction.Left] = true;
        }
        if (cells[1, 1])
        {
            cell = cells[1, 1];
            if (!cells[1, 0] || !cells[1, 1].ConnectedCells[Direction.Left])
                walls[Direction.Top] = true;
            if (!cells[0, 1] || !cells[1, 1].ConnectedCells[Direction.Bottom])
                walls[Direction.Right] = true;

            if (cells[1, 0] && !ReferenceEquals(cells[1, 0].Room, cells[1, 1].Room))
                walls[Direction.Top] = true;

            if (cells[0, 1] && !ReferenceEquals(cells[0, 1].Room, cells[1, 1].Room))
                walls[Direction.Right] = true;
        }

        int wallCount = 0;
        foreach (Direction direction in Direction.Values)
        {
            if (walls[direction])
                wallCount++;
        }
        if (wallCount > 1)
            Instantiate(RoomDecorations.GetGameObject(defaultRoom.connectorWalls), cell.transform).transform.position = pos + new Vector2(0.5f, 0.5f);
    }

    private void DecorateCell(Cell cell, bool[,] decorated)
    {
        decorated[cell.Position.y, cell.Position.x] = true;
        Instantiate(backgroundPrefab, cell.transform);
        foreach (Direction direction in Direction.Values)
        {
            if (cell.AdjacentCells[direction] is null || !decorated[cell.AdjacentCells[direction].Position.y, cell.AdjacentCells[direction].Position.x])
            {
                if (cell.ConnectedCells[direction] is null)
                {
                    if (direction == Direction.Top)
                        Instantiate(horizontalWallPrefab, cell.transform).transform.position += new Vector3(0f, 0.5f, 0f);
                    else if (direction == Direction.Right)
                        Instantiate(verticalWallPrefab, cell.transform).transform.position += new Vector3(0.5f, 0f, 0f);
                    else if (direction == Direction.Left)
                        Instantiate(verticalWallPrefab, cell.transform).transform.position += new Vector3(-0.5f, 0f, 0f);
                    else if (direction == Direction.Bottom)
                        Instantiate(horizontalWallPrefab, cell.transform).transform.position += new Vector3(0f, -0.5f, 0f);
                }
                else if (!ReferenceEquals(cell.Room, cell.ConnectedCells[direction].Room))
                {
                    GameObject door = null;
                    if (direction == Direction.Top)
                    {
                        door = Instantiate(trapdoorPrefab, cell.transform);
                        door.transform.position += new Vector3(0f, 0.5f, 0f);
                    }
                    else if (direction == Direction.Right)
                    {
                        door = Instantiate(doorPrefab, cell.transform);
                        door.transform.position += new Vector3(0.5f, 0f, 0f);
                    }
                    else if (direction == Direction.Left)
                    {
                        door = Instantiate(doorPrefab, cell.transform);
                        door.transform.position += new Vector3(-0.5f, 0f, 0f);
                    }
                    else if (direction == Direction.Bottom)
                    {
                        door = Instantiate(trapdoorPrefab, cell.transform);
                        door.transform.position += new Vector3(0f, -0.5f, 0f);
                    }
                    cell.Walls[direction] = door;
                    if (cell.AdjacentCells[direction] != null)
                        cell.AdjacentCells[direction].Walls[direction.Opposite] = door;
                }
                else if (direction == Direction.Top || direction == Direction.Bottom)
                {
                    int key = 0;

                    if (!cell.ConnectedCells[Direction.Left] || !ReferenceEquals(cell.ConnectedCells[Direction.Left].Room, cell.Room) || !cell.ConnectedCells[direction].ConnectedCells[Direction.Left] 
                        || !ReferenceEquals(cell.ConnectedCells[direction].ConnectedCells[Direction.Left].Room, cell.ConnectedCells[direction].Room))
                        key = 2;

                    if (!cell.ConnectedCells[Direction.Right] || !ReferenceEquals(cell.ConnectedCells[Direction.Right].Room, cell.Room) || !cell.ConnectedCells[direction].ConnectedCells[Direction.Right]
                        || !ReferenceEquals(cell.ConnectedCells[direction].ConnectedCells[Direction.Right].Room, cell.ConnectedCells[direction].Room))
                        key += 1;

                    if (platforms.TryGetValue(key, out GameObject gameObject))
                        Instantiate(gameObject, cell.transform).transform.position += (Vector3)(direction.Rotation2 / 2f);
                }
            }
        }
    }
}