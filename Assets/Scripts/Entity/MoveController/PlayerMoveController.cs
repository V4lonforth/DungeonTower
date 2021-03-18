using DungeonTower.Controllers;
using DungeonTower.Entity.Base;
using DungeonTower.Input;
using DungeonTower.Level.Base;
using DungeonTower.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.Entity.MoveController
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

        protected void CheckMove(Direction direction)
        {
            CheckMove(stage.GetCellSafe(direction.ShiftPosition(cellEntity.Cell.StagePosition)));
        }

        public void CheckMove(Cell cell)
        {
            if (!moveSelected)
            {
                List<ActionOption> actionOptions = GetActionOptions(cell);
                if (actionOptions.Count > 0)
                {
                    moveSelected = true;
                    OnMoveSelected?.Invoke(this, actionOptions[0]);
                }
            }
        }
    }
}
