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
        public override bool CanTarget(Cell from, Cell to)
        {
            return to != null && (from.StagePosition - to.StagePosition).ManhattanDistance() == 1;
        }

        public override List<Cell> GetAvailableTargets(Cell from)
        {
            List<Cell> cells = new List<Cell>();
            foreach (Direction direction in Direction.Values)
            {
                Cell cell = from.Stage.GetCell(from, direction);
                if (cell != null)
                    cells.Add(cell);
            }

            return cells;
        }
    }
}
