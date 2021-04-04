using System;
using System.Collections.Generic;
using DungeonTower.Entity.Health;
using DungeonTower.Entity.MoveControllers;
using DungeonTower.Level.Base;
using DungeonTower.UI;
using UnityEngine;

namespace DungeonTower.Entity.Action
{
    public abstract class MultipleTurnEntityAction : EntityAction
    {
        public Action<ActionOption> OnActionLocked { get; set; }
        public Action<ActionOption> OnActionUnlocked { get; set; }

        [SerializeField] private GameObject intentionIconPrefab;
        [SerializeField] private int cooldown;
        
        private ActionOption lockedAction;
        private GameObject intentionIcon;

        private List<GameObject> dangerHighlights;
        private int remainingCooldown;
        
        private static readonly Vector3 IconOffset = new Vector3(0f, 0.5f, 0f);

        protected new void Awake()
        {
            base.Awake();

            GetComponent<MoveController>().OnMoveStarted += TickCooldown;
            GetComponent<EntityHealth>().OnDeath += h => 
                EntityActionUIController.Instance.RemoveHighlightDanger(dangerHighlights);
        }

        private void TickCooldown(MoveController moveController)
        {
            if (remainingCooldown > 0)
                remainingCooldown--;
        }
        
        public override bool CanInteract(Cell cell)
        {
            return lockedAction != null || (remainingCooldown <= 0 && base.CanInteract(cell));
        }

        public override void Interact(Cell cell)
        {
            base.Interact(cell);
            if (lockedAction == null)
            {
                StartAction(cell);
            }
            else
            {
                ContinueAction(lockedAction.Target);
            }
        }

        private void StartAction(Cell target)
        {
            lockedAction = new ActionOption(this, target);
            intentionIcon = Instantiate(intentionIconPrefab, transform);
            intentionIcon.transform.position += IconOffset;
            OnActionLocked?.Invoke(lockedAction);

            dangerHighlights = EntityActionUIController.Instance.HighlightDanger(GetHighlightCells(target));
            
            StartAction();
            FinishMove(target);
        }

        protected void FinishAction()
        {
            remainingCooldown = cooldown;
            
            FinishMove(lockedAction.Target);
            
            EntityActionUIController.Instance.RemoveHighlightDanger(dangerHighlights);
            dangerHighlights = null;
            
            OnActionUnlocked?.Invoke(lockedAction);
            Destroy(intentionIcon);
            intentionIcon = null;
            lockedAction = null;
        }

        protected abstract void StartAction();
        protected abstract void ContinueAction(Cell target);
        protected abstract List<Cell> GetHighlightCells(Cell target);
    }
}