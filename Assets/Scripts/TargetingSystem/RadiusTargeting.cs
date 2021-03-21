using DungeonTower.Entity.Base;
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

        public override bool CanTarget(CellEntity cellEntity, Cell target)
        {
            return target != null && (cellEntity.Cell.StagePosition - target.StagePosition).ManhattanDistance() <= radius;
        }

        public override List<Cell> GetAvailableTargets(CellEntity cellEntity)
        {
            List<Cell> cells = new List<Cell>();
            foreach (Cell cell in cellEntity.Cell.Stage.Cells)
                if (CanTarget(cellEntity, cell))
                    cells.Add(cell);
            return cells;
        }
    }
}
