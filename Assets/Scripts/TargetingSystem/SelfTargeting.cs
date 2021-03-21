using DungeonTower.Entity.Base;
using DungeonTower.Level.Base;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.TargetingSystem
{
    [CreateAssetMenu(fileName = "Data", menuName = "Targeting/SelfTargeting", order = 1)]
    public class SelfTargeting : Targeting
    {
        public override bool CanTarget(CellEntity cellEntity, Cell target)
        {
            return cellEntity.Cell == target;
        }

        public override List<Cell> GetAvailableTargets(CellEntity cellEntity)
        {
            return new List<Cell>() { cellEntity.Cell };
        }
    }
}
