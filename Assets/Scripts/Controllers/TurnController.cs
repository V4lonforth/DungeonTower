using DungeonTower.Entity.Movement;
using DungeonTower.Level.Base;
using DungeonTower.Utils;
using System;
using System.Collections.Generic;
using DungeonTower.Entity.Action;
using DungeonTower.Entity.Attack;
using DungeonTower.Entity.Health;
using DungeonTower.Entity.MoveControllers;
using UnityEngine;

namespace DungeonTower.Controllers
{
    public class TurnController : Singleton<TurnController>
    {
        public Action OnTurnStart { get; set; }
        public Action OnTurnFinish { get; set; }

        public MoveController PlayerController { get; private set; }

        private readonly List<MoveController> registeredEnemies = new List<MoveController>();
        private readonly List<MoveController> enemiesToMove = new List<MoveController>();
        private readonly List<MoveController> forcedEnemiesToMove = new List<MoveController>();

        private Stage stage;
        private ActionOption savedPlayerAction;

        private void Awake()
        {
            GameController.Instance.OnStageStart += StartStage;
        }

        private void StartStage(Stage stage)
        {
            this.stage = stage;
            SetPlayer(stage.PlayerEntity.GetComponent<MoveController>());
            StartTurn();
        }

        private void EnterState(TurnState state)
        {
            switch (state)
            {
                case TurnState.Turn:
                    StartTurn(); 
                    break;
                case TurnState.PlayerTurn:
                    StartPlayerMove();
                    break;
                case TurnState.EnemyTurn:
                    StartEnemiesMove();
                    break;
                case TurnState.ForcedEnemyTurn:
                    StartForcedEnemiesMove();
                    break;
            }
        }

        private void StartTurn()
        {
            enemiesToMove.AddRange(registeredEnemies);
            EnterState(TurnState.PlayerTurn);
        }

        private void StartPlayerMove()
        {
            PlayerController.OnMoveSelected += CheckPlayerMove;
            PlayerController.StartMove();
        }

        private void CheckPlayerMove(MoveController playerController, ActionOption actionOption)
        {
            playerController.OnMoveSelected -= CheckPlayerMove;
            if (actionOption.EntityAction is IMovementController)
            {
                savedPlayerAction = actionOption;
                EnterState(TurnState.ForcedEnemyTurn);
            }
            else
            {
                MakePlayerMove(actionOption);
            }
        }

        private void MakePlayerMove(ActionOption actionOption)
        {
            PlayerController.OnMoveFinished += FinishPlayerMove;
            PlayerController.MakeMove(actionOption);
        }

        private void FinishPlayerMove(MoveController playerController)
        {
            playerController.OnMoveFinished -= FinishPlayerMove;
            EnterState(TurnState.EnemyTurn);
        }

        private void StartEnemiesMove()
        {
            if (enemiesToMove.Count == 0)
            {
                EnterState(TurnState.Turn);
                return;
            }

            enemiesToMove.Sort((a, b) => stage.Navigator.GetDistanceToPlayer(a.CellEntity.Cell).CompareTo(stage.Navigator.GetDistanceToPlayer(b.CellEntity.Cell)));
            foreach (MoveController enemyController in new List<MoveController>(enemiesToMove))
            {
                if (enemyController != null)
                {
                    enemyController.OnMoveSelected += MakeEnemyMove;
                    enemyController.StartMove();
                }
            }
        }

        private void MakeEnemyMove(MoveController enemyController, ActionOption actionOption)
        {
            enemyController.OnMoveSelected -= MakeEnemyMove;
            enemyController.OnMoveFinished += FinishEnemyMove;
            enemyController.MakeMove(actionOption);
        }

        private void FinishEnemyMove(MoveController enemyController)
        {
            enemyController.OnMoveFinished -= FinishEnemyMove;
            enemiesToMove.Remove(enemyController);
            if (enemiesToMove.Count == 0)
            {
                EnterState(TurnState.Turn);
            }
        }

        private void StartForcedEnemiesMove()
        {
            foreach (Direction direction in Direction.Values)
            {
                Cell adjacentCell = stage.GetCell(PlayerController.CellEntity.Cell, direction);
                if (adjacentCell != null && adjacentCell.FrontEntity != null && stage.Navigator.CheckPath(adjacentCell.FrontEntity, adjacentCell, direction.Opposite))
                {
                    MoveController enemyController = adjacentCell.FrontEntity.GetComponent<MoveController>();
                    if (enemyController != null && enemyController.Active && !enemyController.Sleeping)
                    {
                        forcedEnemiesToMove.Add(enemyController);
                    }
                }
            }

            if (forcedEnemiesToMove.Count == 0)
            {
                MakePlayerMove(savedPlayerAction);
                savedPlayerAction = null;
                return;
            }

            foreach (MoveController enemyController in new List<MoveController>(forcedEnemiesToMove))
            {
                enemyController.OnMoveSelected += MakeForcedEnemiesMove;
                enemyController.StartMove();
            }
        }

        private void MakeForcedEnemiesMove(MoveController enemyController, ActionOption actionOption)
        {
            enemyController.OnMoveSelected -= MakeForcedEnemiesMove;
            if (actionOption?.EntityAction is EntityAttack)
            {
                enemyController.OnMoveFinished += FinishForcedEnemyMove;
                enemyController.MakeMove(actionOption);
            }
            else
            {
                forcedEnemiesToMove.Remove(enemyController);
                
                if (forcedEnemiesToMove.Count == 0)
                {
                    if (PlayerController != null)
                        MakePlayerMove(savedPlayerAction);
                    savedPlayerAction = null;
                }
            }
        }

        private void FinishForcedEnemyMove(MoveController enemyController)
        {
            enemyController.OnMoveFinished -= FinishForcedEnemyMove;
            forcedEnemiesToMove.Remove(enemyController);
            enemiesToMove.Remove(enemyController);

            if (forcedEnemiesToMove.Count == 0)
            {
                if (PlayerController != null)
                    MakePlayerMove(savedPlayerAction);
                savedPlayerAction = null;
            }
        }

        public void SetPlayer(MoveController moveController)
        {
            PlayerController = moveController;
            PlayerController.GetComponent<EntityHealth>().OnDeath += FinishGame;
        }
        public void RemovePlayer(MoveController moveController)
        {
            if (moveController == PlayerController)
            {
                PlayerController = null;
            }
        }

        public void AddEnemy(MoveController moveController)
        {
            registeredEnemies.Add(moveController);
        }
        public void RemoveEnemy(MoveController moveController)
        {
            enemiesToMove.Remove(moveController);
            registeredEnemies.Remove(moveController);
        }

        private void FinishGame(EntityHealth entityHealth)
        {
            GameController.Instance.FinishGame();
        }
    }
}
