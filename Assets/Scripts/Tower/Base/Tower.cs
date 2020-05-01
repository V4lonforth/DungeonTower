using System.Collections.Generic;
using UnityEngine;

public class Tower
{
    public Cell this[Vector2Int pos]
    {
        get => this[pos.y, pos.x];
        set => this[pos.y, pos.x] = value;
    }
    public Cell this[int y, int x]
    {
        get => Cells[y, x];
        set => Cells[y, x] = value;
    }

    public Vector2Int Size { get; private set; }
    public Cell[,] Cells { get; private set; }
    public List<Room> Rooms { get; private set; }

    public Player Player { get; set; }
    public List<Enemy> Enemies { get; set; }

    public TowerGenerator TowerGenerator { get; private set; }
    public Navigator Navigator { get; private set; }

    public Tower(TowerGenerator towerGenerator)
    {
        TowerGenerator = towerGenerator;
        Size = TowerGenerator.size;
        Cells = new Cell[Size.y, Size.x];
        Rooms = new List<Room>();

        Navigator = new Navigator(this);
    }

    public Vector2Int WorldToTowerPoint(Vector2 position)
    {
        return (Vector2Int)TowerGenerator.floorTilemap.WorldToCell(position - Vector2.one / 2f);
    }
}