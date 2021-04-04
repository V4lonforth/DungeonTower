using DungeonTower.Controllers;
using DungeonTower.Entity.Base;
using DungeonTower.Input;
using DungeonTower.Level.Base;
using DungeonTower.Utils;
using System.Collections.Generic;
using DungeonTower.Entity.Action;
using UnityEngine;

namespace DungeonTower.Entity.MoveControllers
{
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(CellEntity))]
    public class PlayerMoveController : MoveController
    {
        private CellEntity cellEntity;
        private bool moveSelected;

        protected new void Awake()
        {
            base.Awake();

            cellEntity = GetComponent<CellEntity>();

            PlayerInputHandler playerInputHandler = GetComponent<PlayerInputHandler>();
            playerInputHandler.OnSwipe += CheckMove;
            playerInputHandler.OnPress += CheckMove;

            GameController.Instance.OnStageStart += s => Activate();
        }

        protected override void SelectMove()
        {
            moveSelected = false;
        }

        private void CheckMove(Direction direction)
        {
            CheckMove(stage.GetCellSafe(direction.ShiftPosition(cellEntity.Cell.StagePosition)));
        }

        public void CheckMove(Cell cell)
        {
            if (!moveSelected)
            {
                ActionOption actionOption = GetActionOption(cell);
                if (actionOption != null)
                {
                    moveSelected = true;
                    SelectMove(actionOption);
                }
            }
        }
        
        private ActionOption GetActionOption(Cell target)
        {
            List<ActionOption> actionOptions = new List<ActionOption>();

            if (SelectedAction != null)
            {
                if (SelectedAction.CanInteract(target))
                {
                    actionOptions.Add(new ActionOption(SelectedAction, target));
                }
            }
            else
            {
                foreach (EntityAction entityController in entityActions)
                {
                    if (!entityController.RequiredSelection && entityController.CanInteract(target))
                    {
                        actionOptions.Add(new ActionOption(entityController, target));
                    }
                }
            }

            actionOptions.Sort((a, b) => b.EntityAction.Priority.CompareTo(a.EntityAction.Priority));
            return actionOptions.Count > 0 ? actionOptions[0] : null;
        }
    }
}
