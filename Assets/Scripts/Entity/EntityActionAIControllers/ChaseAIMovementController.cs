using DungeonTower.Entity.Action;
using DungeonTower.Entity.Base;
using DungeonTower.Entity.Movement;
using DungeonTower.Level.Base;
using DungeonTower.Utils;
using UnityEngine;

namespace DungeonTower.Entity.EntityActionAIControllers
{
    public class ChaseAIMovementController : MonoBehaviour, IEntityActionAIController
    {
        private MovementController movementController;
        private CellEntity cellEntity;
        
        private void Awake()
        {
            movementController = GetComponent<MovementController>();
            cellEntity = GetComponent<CellEntity>();
        }

        public ActionOption GetAction()
        {
            foreach (Direction direction in cellEntity.Cell.Stage.Navigator.GetDirectionsToPlayer(cellEntity.Cell))
            {
                Cell target = cellEntity.Cell.Stage.GetCell(cellEntity.Cell, direction);
                if (target != null && movementController.CanInteract(target))
                    return new ActionOption(movementController, target);
            }

            return null;
        }
    }
}