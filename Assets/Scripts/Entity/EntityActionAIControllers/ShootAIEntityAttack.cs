using DungeonTower.Controllers;
using DungeonTower.Entity.Action;
using DungeonTower.Entity.Attack;
using DungeonTower.Entity.Base;
using DungeonTower.Level.Base;
using UnityEngine;

namespace DungeonTower.Entity.EntityActionAIControllers
{
    public class ShootAIEntityAttack : MonoBehaviour, IEntityActionAIController
    {
        private PreciseShotEntityAttack preciseShotEntityAttack;
        private CellEntity cellEntity;
        
        private void Awake()
        {
            preciseShotEntityAttack = GetComponent<PreciseShotEntityAttack>();
            cellEntity = GetComponent<CellEntity>();
        }
        
        public ActionOption GetAction()
        {
            Cell target = TurnController.Instance.PlayerController.CellEntity.Cell;
            return preciseShotEntityAttack.CanInteract(target) ? new ActionOption(preciseShotEntityAttack, target) : null;
        }
    }
}