using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Tower Tower { get; private set; }
    public List<Cell> Cells { get; private set; }

    public int LowestLevel { get; private set; }
    public int HighestLevel { get; private set; }

    public List<Room> AdjacentRooms { get; private set; }
    public List<Room> ConnectedRooms { get; private set; }

    public bool IsRevealed { get; set; }
    public GameObject FogOfWar { get; set; }

    public static Room Instantiate(GameObject roomPrefab, Tower tower)
    {
        Room room = Instantiate(roomPrefab, tower.transform).GetComponent<Room>();
        room.Tower = tower;
        return room;
    }

    private void Awake()
    {
        Cells = new List<Cell>();

        AdjacentRooms = new List<Room>();
        ConnectedRooms = new List<Room>();
    }

    public void AddCell(Cell cell)
    {
        Cells.Add(cell);

        if (Cells.Count == 0)
            LowestLevel = HighestLevel = cell.Position.y;
        else
        {
            LowestLevel = Mathf.Min(LowestLevel, cell.Position.y);
            HighestLevel = Mathf.Max(HighestLevel, cell.Position.y);
        }
    }

    public void AggroEnemies()
    {
        foreach (Cell cell in Cells)
        {
            if (cell.CreatureEntity is EnemyEntity enemy)
                enemy.Aggro();
        }
    }
}