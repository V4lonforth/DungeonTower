using DungeonTower.Controllers;
using DungeonTower.Entity.Health;
using DungeonTower.Entity.Action;
using DungeonTower.Entity.EntityActionAIControllers;
using System.Collections.Generic;

namespace DungeonTower.Entity.MoveControllers
{
    public class EnemyMoveController : MoveController
    {
        private IEntityActionAIController[] aiControllers;
        
        protected new void Awake()
        {
            base.Awake();

            aiControllers = GetComponents<IEntityActionAIController>();
            
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
            List<ActionOption> actionOptions = new List<ActionOption>();
            foreach (IEntityActionAIController aiController in aiControllers)
            {
                ActionOption actionOption = aiController.GetAction();
                if (actionOption != null)
                    actionOptions.Add(actionOption);
            }
            
            actionOptions.Sort((a, b) => b.EntityAction.Priority.CompareTo(a.EntityAction.Priority));
            if (actionOptions.Count > 0)
                return actionOptions[0];
            return null;
        }

        protected override void SelectMove()
        {
            SelectMove(SelectActionOption());
        }

        private void Die(EntityHealth entityHealth)
        {
            TurnController.Instance.RemoveEnemy(this);
        }
    }
}
