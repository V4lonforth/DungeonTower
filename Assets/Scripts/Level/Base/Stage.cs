using DungeonTower.Entity.Base;
using DungeonTower.Level.StageController;
using DungeonTower.Utils;
using UnityEngine;

namespace DungeonTower.Level.Base
{
    public class Stage
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

        public Vector2Int Size { get; }
        public Room[] Rooms { get; }
        public Cell[,] Cells { get; }
        public CellEntity PlayerEntity { get; set; }

        public Navigator Navigator { get; }
        public FogOfWarController FogOfWarController { get; }

        public Stage(Vector2Int size, Cell[,] cells, Room[] rooms, Navigator navigator, FogOfWarController fogOfWarController)
        {
            Size = size;
            Rooms = rooms;
            Cells = cells;
            Navigator = navigator;
            FogOfWarController = fogOfWarController;

            foreach (Cell cell in cells)
                cell.Stage = this;
        }

        public Vector2Int WorldToTowerPoint(Vector2 position)
        {
            return new Vector2Int(Mathf.FloorToInt(position.x + 0.5f), Mathf.FloorToInt(position.y + 0.5f));
        }

        public Cell GetCellSafe(Vector2Int position)
        {
            if (MathHelper.InRange(position, Size))
                return this[position];
            else
                return null;
        }

        public Cell GetCell(Cell cell, Direction direction)
        {
            return GetCellSafe(direction.ShiftPosition(cell.StagePosition));
        }
    }
}