using DungeonTower.Level.Base;
using DungeonTower.Level.Generation.Walls;
using DungeonTower.Utils;
using UnityEngine;

namespace DungeonTower.Level.StageController
{
    public class WallController
    {
        public WallType[,] VerticalWalls { get; }
        public WallType[,] HorizontalWalls { get; }

        public WallController(WallType[,] verticalWalls, WallType[,] horizontalWalls)
        {
            VerticalWalls = verticalWalls;
            HorizontalWalls = horizontalWalls;
        }

        public WallType GetWall(Cell from, Cell to)
        {
            Direction direction = Direction.GetDirection(from.StagePosition, to.StagePosition);

            if (direction == null)
                return WallType.None;

            Vector2Int fromPosition = from.StagePosition;
            if (direction == Direction.Left || direction == Direction.Bottom)
            {
                fromPosition = direction.ShiftPosition(fromPosition);
                direction = direction.Opposite;
            }

            if (direction == Direction.Top)
                return HorizontalWalls[fromPosition.y, fromPosition.x];
            if (direction == Direction.Right)
                return VerticalWalls[fromPosition.y, fromPosition.x];
            return WallType.None;
        }

        public bool HasWall(Cell from, Cell to)
        {
            return GetWall(from, to) == WallType.Wall;
        }
    }
}
