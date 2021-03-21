using DungeonTower.Entity.Base;
using DungeonTower.Level.Base;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.TargetingSystem
{
    public abstract class Targeting : ScriptableObject
    {
        public abstract bool CanTarget(CellEntity cellEntity, Cell target);
        public abstract List<Cell> GetAvailableTargets(CellEntity cellEntity);
    }
}
