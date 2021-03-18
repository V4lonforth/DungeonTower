using UnityEngine;

namespace DungeonTower.Level.Base
{
    public class Room
    {
        public Cell[] Cells { get; }

        public Room(Cell[] cells)
        {
            Cells = cells;
        }
    }
}