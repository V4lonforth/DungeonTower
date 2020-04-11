﻿using System.Collections.Generic;
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
    public List<Room> Rooms { get; private set; }
    public Cell[,] Cells { get; private set; }

    public Player Player { get; set; }

    public TowerGenerator TowerGenerator { get; private set; }
    public Navigator Navigator { get; private set; }

    public Tower(Vector2Int size, TowerGenerator towerGenerator)
    {
        Cells = new Cell[size.y, size.x];
        Rooms = new List<Room>();
        Size = size;
        Navigator = new Navigator(this);
        TowerGenerator = towerGenerator;
    }

    public void Interact(Vector2Int position)
    {
        if (MathHelper.InRange(position, Size))
        {
            Player.SetTarget(Cells[position.y, position.x]);
        }
    }
}