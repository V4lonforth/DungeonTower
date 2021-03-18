using DungeonTower.Level.Base;
using DungeonTower.Level.Generation.Rooms;
using DungeonTower.Level.StageController;
using DungeonTower.Utils;
using System.Linq;
using UnityEngine;

namespace DungeonTower.Level.Generation.Walls
{
    [CreateAssetMenu(fileName = "Data", menuName = "TowerGeneration/WallConstructor", order = 1)]
    public class WallConstructor : ScriptableObject
    {
        public WallController Construct(Room[] rooms, Vector2Int stageSize, RoomConnection[] roomConnections)
        {
            WallType[,] verticalWalls = new WallType[stageSize.y, stageSize.x - 1];
            WallType[,] horizontalWalls = new WallType[stageSize.y - 1, stageSize.x];

            foreach (Room room in rooms)
            {
                foreach (Cell cell in room.Cells)
                {
                    Vector2Int topCellPosition = Direction.Top.ShiftPosition(cell.StagePosition);
                    if (MathHelper.InRange(topCellPosition, stageSize) && !room.Cells.Any(c => c.StagePosition == topCellPosition))
                        horizontalWalls[cell.StagePosition.y, cell.StagePosition.x] = WallType.Wall;

                    Vector2Int rightCellPosition = Direction.Right.ShiftPosition(cell.StagePosition);
                    if (MathHelper.InRange(rightCellPosition, stageSize) && !room.Cells.Any(c => c.StagePosition == rightCellPosition))
                        verticalWalls[cell.StagePosition.y, cell.StagePosition.x] = WallType.Wall;
                }
            }

            foreach (RoomConnection roomConnection in roomConnections)
            {
                Vector2Int fromPosition = roomConnection.Cell.StagePosition;
                
                if (roomConnection.Direction == Direction.Right)
                    verticalWalls[fromPosition.y, fromPosition.x] = WallType.Door;

                if (roomConnection.Direction == Direction.Top)
                    horizontalWalls[fromPosition.y, fromPosition.x] = WallType.Door;
            }

            return new WallController(verticalWalls, horizontalWalls);
        }
    }
}
