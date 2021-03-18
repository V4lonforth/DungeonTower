using DungeonTower.Controllers;
using DungeonTower.Entity.Base;
using DungeonTower.Entity.Health;
using DungeonTower.Level.Base;
using DungeonTower.Utils;
using System.Collections.Generic;

namespace DungeonTower.Entity.MoveController
{
    public class EnemyMoveController : MoveController
    {
        protected new void Awake()
        {
            base.Awake();

            EntityHealth entityHealth = GetComponent<EntityHealth>();
            if (entityHealth != null)
                entityHealth.OnDeath += Die;
        }

        public override void Activate()
        {
            base.Activate();
            TurnController.Instance.AddEnemy(this);
        }

        private ActionOption SelectActionOption()
        {
            CellEntity cellEntity = GetComponent<CellEntity>();
            Cell currentCell = cellEntity.Cell;

            List<ActionOption> actionOptions = new List<ActionOption>();
                 
            foreach (Direction direction in stage.Navigator.GetDirectionsToPlayer(currentCell))
            {
                Cell target = stage.GetCell(cellEntity.Cell, direction);
                if (target != null)
                    actionOptions.AddRange(GetActionOptions(target));
            }

            actionOptions.Sort((a, b) => b.EntityAction.Priority.CompareTo(a.EntityAction.Priority));
            if (actionOptions.Count > 0)
                return actionOptions[0];
            return null;
        }

        protected override void SelectMove()
        {
            ActionOption actionOption = SelectActionOption();
            OnMoveSelected?.Invoke(this, actionOption);
        }

        private void Die(EntityHealth entityHealth)
        {
            TurnController.Instance.RemoveEnemy(this);
        }
    }
}
