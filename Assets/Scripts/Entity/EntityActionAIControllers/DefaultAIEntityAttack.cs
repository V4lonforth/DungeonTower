using DungeonTower.Entity.Action;
using DungeonTower.Entity.Attack;
using DungeonTower.Entity.Base;
using DungeonTower.Level.Base;
using UnityEngine;

namespace DungeonTower.Entity.EntityActionAIControllers
{
    public class DefaultAIEntityAttack : MonoBehaviour, IEntityActionAIController
    {
        private EntityAttack entityAttack;
        private CellEntity cellEntity;
        
        private void Awake()
        {
            entityAttack = GetComponent<EntityAttack>();
            cellEntity = GetComponent<CellEntity>();
        }
        
        public ActionOption GetAction()
        {
            Cell target = cellEntity.Cell.Stage.GetCellSafe(cellEntity.Cell.Stage.Navigator.GetDirectionToPlayer(cellEntity.Cell).ShiftPosition(cellEntity.Cell.StagePosition));
            return entityAttack.CanInteract(target) ? new ActionOption(entityAttack, target) : null;
        }
    }
}