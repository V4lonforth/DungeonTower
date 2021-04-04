using DungeonTower.Controllers;
using DungeonTower.Entity.Action;
using DungeonTower.Entity.Base;
using DungeonTower.Entity.CellBorder;
using DungeonTower.Level.Base;
using DungeonTower.Utils;
using System;
using UnityEngine;

namespace DungeonTower.Entity.MoveControllers
{
    [RequireComponent(typeof(CellEntity))]
    public abstract class MoveController : MonoBehaviour
    {
        public Action<MoveController> OnMoveStarted { get; set; }
        public Action<MoveController, ActionOption> OnMoveSelected { get; set; }
        public Action<MoveController, ActionOption> OnMoveMade { get; set; }
        public Action<MoveController> OnMoveFinished { get; set; }

        public Action<MoveController, EntityAction> OnActionSelected { get; set; }
        public Action<MoveController, EntityAction> OnActionDeselected { get; set; }

        public MoveState MoveState { get; private set; }
        public bool Active { get; private set; }
        public bool Sleeping { get; set; }

        public CellEntity CellEntity { get; private set; }

        public ActionOption LockedAction { get; private set; }
        public EntityAction SelectedAction { get; private set; }
        protected Stage stage;

        protected EntityAction[] entityActions;

        protected void Awake()
        {
            CellEntity = GetComponent<CellEntity>();
            entityActions = GetComponents<EntityAction>();
            GameController.Instance.OnStageStart += StartStage;

            foreach (MultipleTurnEntityAction action in  GetComponents<MultipleTurnEntityAction>())
            {
                action.OnActionLocked += LockAction;
                action.OnActionUnlocked += UnlockAction;
            }
        }

        private void StartStage(Stage stage)
        {
            this.stage = stage;
        }

        public virtual void Activate()
        {
            Active = true;
        }

        public void StartMove()
        {
            MoveState = MoveState.Started;
            OnMoveStarted?.Invoke(this);

            if (Sleeping)
            {
                OnMoveSelected?.Invoke(this, null);
            } 
            else
            {
                SelectMove();
            }
        }

        public void SelectMove(ActionOption actionOption)
        {
            OnMoveSelected?.Invoke(this, actionOption);
        }

        public void MakeMove(ActionOption actionOption)
        {
            MoveState = MoveState.MakingMove;
            OnMoveMade?.Invoke(this, actionOption);

            if (actionOption == null)
            {
                FinishMove(null);
                return;
            }

            Direction direction = Direction.GetDirection(CellEntity.Cell.StagePosition, actionOption.Target.StagePosition);
            if (direction != null && CellEntity.Cell.BorderEntities[direction])
                CellEntity.Cell.BorderEntities[direction].GetComponent<IInteractableThrough>()?.Interact(CellEntity, direction);

            actionOption.EntityAction.OnMoveFinish += FinishMove;
            actionOption.EntityAction.Interact(actionOption.Target);

            DeselectAction();
        }

        protected void FinishMove(EntityAction entityController)
        {
            MoveState = MoveState.Finished;
            OnMoveFinished?.Invoke(this);

            if (entityController != null)
                entityController.OnMoveFinish -= FinishMove;
        }
        
        public void SelectAction(EntityAction entityAction)
        {
            if (SelectedAction != null && SelectedAction != entityAction)
                DeselectAction();

            SelectedAction = entityAction;
            OnActionSelected?.Invoke(this, entityAction);
        }

        public void DeselectAction()
        {
            if (SelectedAction != null)
            {
                OnActionDeselected?.Invoke(this, SelectedAction);
                SelectedAction = null;
            }
        }

        private void LockAction(ActionOption actionOption)
        {
            LockedAction = actionOption;
        }

        private void UnlockAction(ActionOption actionOption)
        {
            LockedAction = null;
        }

        protected abstract void SelectMove();
    }
}
