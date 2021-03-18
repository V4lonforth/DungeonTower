using DungeonTower.Level.Base;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.TargetingSystem
{
    public abstract class Targeting : ScriptableObject
    {
        public abstract bool CanTarget(Cell from, Cell to);
        public abstract List<Cell> GetAvailableTargets(Cell from);
    }
}
