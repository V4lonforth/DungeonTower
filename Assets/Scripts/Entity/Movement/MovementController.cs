using DungeonTower.Entity.Action;
using DungeonTower.Entity.CellBorder;
using DungeonTower.Level.Base;
using DungeonTower.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace DungeonTower.Entity.Movement
{
    public class MovementController : EntityAction, IMovementController
    {
        [SerializeField] private float movementTime;
        
        public Action<IMovementController, Cell, Cell> OnMovement { get; set; }

        public override bool CanInteract(Cell cell)
        {
            return base.CanInteract(cell) && cell.FrontEntity == null && stage.Navigator.CheckPath(cellEntity, cellEntity.Cell, cell);
        }

        public override void Interact(Cell cell)
        {
            base.Interact(cell);
            OnMovement?.Invoke(this, cellEntity.Cell, cell);

            Direction direction = Direction.GetDirection(cellEntity.Cell.StagePosition, cell.StagePosition);
            if (cellEntity.Cell.BorderEntities[direction])
                cellEntity.Cell.BorderEntities[direction].GetComponent<IPassable>()?.Pass(cellEntity, direction);

            cellEntity.Detach();
            cellEntity.Attach(cell);
            StartCoroutine(MoveToCell(movementTime, cell));
        }

        protected virtual IEnumerator MoveToCell(float movingTimeLeft, Cell cell)
        {
            while (movingTimeLeft > 0f)
            {
                if (Time.deltaTime >= movingTimeLeft)
                {
                    movingTimeLeft = 0f;
                    FinishMove(cell);
                }
                else
                {
                    transform.position += ((Vector3)cell.WorldPosition - transform.position) * (Time.deltaTime / movingTimeLeft);
                    movingTimeLeft -= Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
}
