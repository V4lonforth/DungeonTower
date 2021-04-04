using DungeonTower.Entity.Action;
using DungeonTower.Entity.Base;
using DungeonTower.Entity.Movement;
using DungeonTower.Level.Base;
using DungeonTower.Utils;
using UnityEngine;

namespace DungeonTower.Entity.EntityActionAIControllers
{
    public class RetreatAIMovementController : MonoBehaviour, IEntityActionAIController
    {
        [SerializeField] private int maxDistanceToRetreat;
        
        private MovementController movementController;
        private CellEntity cellEntity;
        
        private void Awake()
        {
            movementController = GetComponent<MovementController>();
            cellEntity = GetComponent<CellEntity>();
        }
        
        public ActionOption GetAction()
        {
            if (cellEntity.Cell.Stage.Navigator.GetDistanceToPlayer(cellEntity.Cell) >= maxDistanceToRetreat)
                return null;
            
            foreach (Direction direction in cellEntity.Cell.Stage.Navigator.GetDirectionsFromPlayer(cellEntity.Cell))
            {
                Cell target = cellEntity.Cell.Stage.GetCell(cellEntity.Cell, direction);
                if (target != null && movementController.CanInteract(target))
                    return new ActionOption(movementController, target);
            }

            return null;
        }
    }
}