using System.Collections.Generic;
using DungeonTower.Controllers;
using DungeonTower.Entity.Base;
using DungeonTower.Entity.CellBorder;
using DungeonTower.Entity.Movement;
using DungeonTower.Level.Base;
using DungeonTower.Utils;
using UnityEngine;

namespace DungeonTower.Level.StageController
{
    public class Navigator
    {
        private readonly Vector2Int stageSize;
        private readonly Cell[,] cells;

        private readonly int[,] distance;
        private readonly bool[,] seen;

        public Navigator(Cell[,] cells)
        {
            this.cells = cells;
            stageSize = MathHelper.GetArraySize(cells);

            distance = new int[stageSize.y, stageSize.x];
            seen = new bool[stageSize.y, stageSize.x];

            GameController.Instance.OnStageStart += StartStage;
        }

        private void StartStage(Stage stage)
        {
            stage.PlayerEntity.GetComponent<IMovementController>().OnMovement += (m, c1, c2) => CreateMap(c2);
        }

        public bool CheckPath(CellEntity cellEntity, Cell first, Cell second)
        {
            if (second == null)
                return false;

            return CheckPath(cellEntity, first, Direction.GetDirection(first.StagePosition, second.StagePosition));
        }

        public bool CheckPath(CellEntity cellEntity, Cell cell, Direction direction)
        {
            if (cell.BorderEntities[direction] != null)
            {
                IPassable passable = cell.BorderEntities[direction].GetComponent<IPassable>();
                if (passable != null)
                    return passable.CanPass(cellEntity);
                return false;
            }
            return true;
        }

        private void ClearMap()
        {
            for (int y = 0; y < stageSize.y; y++)
                for (int x = 0; x < stageSize.x; x++)
                {
                    seen[y, x] = false;
                    distance[y, x] = 0;
                }
        }

        public void CreateMap(Cell startCell)
        {
            ClearMap();

            seen[startCell.StagePosition.y, startCell.StagePosition.x] = true;
            Queue<Cell> queue = new Queue<Cell>();
            foreach (Direction direction in Direction.Values)
            {
                if (CheckPath(null, startCell, direction))
                {
                    Vector2Int nextPosition = direction.ShiftPosition(startCell.StagePosition);
                    Cell cell = cells[nextPosition.y, nextPosition.x];
                    queue.Enqueue(cell);
                }
            }

            while (queue.Count > 0)
            {
                Cell cell = queue.Dequeue();
                if (seen[cell.StagePosition.y, cell.StagePosition.x])
                    continue;
                seen[cell.StagePosition.y, cell.StagePosition.x] = true;

                int minDistance = int.MaxValue;
                foreach (Direction direction in Direction.Values)
                {
                    if (CheckPath(null, cell, direction))
                    {
                        Cell nextCell = cell.Stage.GetCell(cell, direction);
                        if (seen[nextCell.StagePosition.y, nextCell.StagePosition.x])
                        {
                            if (minDistance > distance[nextCell.StagePosition.y, nextCell.StagePosition.x])
                                minDistance = distance[nextCell.StagePosition.y, nextCell.StagePosition.x];
                        }
                        else
                        {
                            queue.Enqueue(nextCell);
                        }
                    }
                }
                distance[cell.StagePosition.y, cell.StagePosition.x] = minDistance + 1;
            }
        }

        public int GetDistanceToPlayer(Cell cell)
        {
            return distance[cell.StagePosition.y, cell.StagePosition.x];
        }

        public Direction GetDirectionToPlayer(Cell cell)
        {
            List<Direction> directions = GetDirectionsToPlayer(cell);
            return directions.Count > 0 ? directions[0] : null;
        }
        
        public List<Direction> GetDirectionsToPlayer(Cell cell)
        {
            List<Direction> directions = new List<Direction>();
            foreach (Direction direction in Direction.Values)
            {
                Vector2Int nextPosition = direction.ShiftPosition(cell.StagePosition);
                if (MathHelper.InRange(nextPosition, stageSize) && distance[cell.StagePosition.y, cell.StagePosition.x] > distance[nextPosition.y, nextPosition.x])
                    directions.Add(direction);
            }
            return directions;
        }

        public List<Direction> GetDirectionsFromPlayer(Cell cell)
        {
            List<Direction> directions = new List<Direction>();
            foreach (Direction direction in Direction.Values)
            {
                Vector2Int nextPosition = direction.ShiftPosition(cell.StagePosition);
                if (MathHelper.InRange(nextPosition, stageSize) && distance[cell.StagePosition.y, cell.StagePosition.x] < distance[nextPosition.y, nextPosition.x])
                    directions.Add(direction);
            }
            return directions;
        }
    }
}