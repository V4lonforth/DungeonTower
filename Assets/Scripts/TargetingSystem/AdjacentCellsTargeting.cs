using DungeonTower.Entity.Base;
using DungeonTower.Level.Base;
using DungeonTower.Utils;
using DungeonTower.Utils.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.TargetingSystem
{
    [CreateAssetMenu(fileName = "Data", menuName = "Targeting/AdjacentCellsTargeting", order = 1)]
    public class AdjacentCellsTargeting : Targeting
    {
        [SerializeField] private bool ignoreWalls;

        public override bool CanTarget(CellEntity cellEntity, Cell target)
        {
            return target != null && (cellEntity.Cell.StagePosition - target.StagePosition).ManhattanDistance() == 1 &&
                (ignoreWalls || cellEntity.Cell.Stage.Navigator.CheckPath(cellEntity, cellEntity.Cell, target));
        }

        public override List<Cell> GetAvailableTargets(CellEntity cellEntity)
        {
            List<Cell> cells = new List<Cell>();
            foreach (Direction direction in Direction.Values)
            {
                Cell cell = cellEntity.Cell.Stage.GetCell(cellEntity.Cell, direction);
                if (cell != null && CanTarget(cellEntity, cell))
                    cells.Add(cell);
            }

            return cells;
        }
    }
}
