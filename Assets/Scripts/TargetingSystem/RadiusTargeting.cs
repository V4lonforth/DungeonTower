using DungeonTower.Level.Base;
using DungeonTower.Utils.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.TargetingSystem
{
    [CreateAssetMenu(fileName = "Data", menuName = "Targeting/RadiusTargeting", order = 1)]
    public class RadiusTargeting : Targeting
    {
        [SerializeField] private int radius;

        public override bool CanTarget(Cell from, Cell to)
        {
            return to != null && (from.StagePosition - to.StagePosition).ManhattanDistance() <= radius;
        }

        public override List<Cell> GetAvailableTargets(Cell from)
        {
            List<Cell> cells = new List<Cell>();
            foreach (Cell cell in from.Stage.Cells)
                if (CanTarget(from, cell))
                    cells.Add(cell);
            return cells;
        }
    }
}
