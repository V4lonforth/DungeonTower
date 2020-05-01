using System.Collections.Generic;

public class Room
{
    public Tower Tower { get; private set; }
    public List<Cell> Cells { get; private set; }
    public List<Cell> DoorCells { get; private set; }
    public List<Cell> NonCutPoints { get; set; }

    public RoomType Type { get; private set; }
    public List<Room> ConnectedRooms { get; private set; }
    public List<Room> AdjacentRooms { get; private set; }

    public bool IsRevealed { get; set; }
    public int Strength { get; set; }
    public int Value { get; set; }

    public Room(Tower tower, RoomType type)
    {
        Tower = tower;
        Type = type;

        Cells = new List<Cell>();
        DoorCells = new List<Cell>();

        ConnectedRooms = new List<Room>();
        AdjacentRooms = new List<Room>();
    }

    public void AggroEnemies()
    {
        foreach (Cell cell in Cells)
        {
            if (cell.Entity is Enemy enemy)
                enemy.Aggro();
        }
    }
}