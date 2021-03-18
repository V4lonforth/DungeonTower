using DungeonTower.Level.Base;
using DungeonTower.Utils;

namespace DungeonTower.Level.Generation.Rooms
{
    public class RoomConnection
    {
        public Cell Cell { get; }
        public Direction Direction { get; }

        public Room From { get; }
        public Room To { get; }

        public RoomConnection(Cell cell, Direction direction, Room from, Room to)
        {
            Cell = cell;
            Direction = direction;
            From = from;
            To = to;
        }
    }
}
