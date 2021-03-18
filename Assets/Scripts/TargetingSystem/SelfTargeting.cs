using DungeonTower.Level.Base;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.TargetingSystem
{
    [CreateAssetMenu(fileName = "Data", menuName = "Targeting/SelfTargeting", order = 1)]
    public class SelfTargeting : Targeting
    {
        public override bool CanTarget(Cell from, Cell to)
        {
            return from == to;
        }

        public override List<Cell> GetAvailableTargets(Cell from)
        {
            return new List<Cell>() { from };
        }
    }
}
