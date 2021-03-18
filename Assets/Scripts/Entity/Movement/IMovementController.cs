using DungeonTower.Level.Base;
using System;

namespace DungeonTower.Entity.Movement
{
    public interface IMovementController
    {
        Action<IMovementController, Cell, Cell> OnMovement { get; set; }
    }
}
