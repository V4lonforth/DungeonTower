using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public enum MoveState
    {
        PreparingMove,
        ForceMove,
        PlayerPreparingMove,
        PlayerMakingMove,
        PlayerFinishingMove,
        EnemiesPreparingMove,
        EnemiesMakingMove,
        EnemiesFinishingMove
    }

    public bool AbleToMakeMove => turnState == MoveState.PlayerMakingMove;
    public bool CurrentlyAnimated => enemiesFinishingMove.Count > 0;
    public Tower Tower { get; set; }

    private MoveState turnState;

    private List<CreatureEntity> enemiesPreparingMove;
    private List<CreatureEntity> enemiesMakingMove;
    private List<CreatureEntity> enemiesFinishingMove;

    private Dictionary<MoveState, Action> turnActions;

    private void Awake()
    {
        enemiesPreparingMove = new List<CreatureEntity>();
        enemiesMakingMove = new List<CreatureEntity>();
        enemiesFinishingMove = new List<CreatureEntity>();

        turnActions = new Dictionary<MoveState, Action>()
        {
            { MoveState.PreparingMove, PreapareMove },
            { MoveState.ForceMove, FinishForceMoves },
            { MoveState.PlayerPreparingMove, PreparePlayerMove },
            { MoveState.PlayerMakingMove, MakePlayerMove },
            { MoveState.PlayerFinishingMove, FinishPlayerMove },
            { MoveState.EnemiesPreparingMove, PrepareEnemiesMove },
            { MoveState.EnemiesMakingMove, MakeEnemiesMove },
            { MoveState.EnemiesFinishingMove, FinishEnemiesMove }
        };
    }

    private void Update()
    {
        if (turnActions.TryGetValue(turnState, out Action action))
            action();
    }

    public void StartLevel()
    {
        PreapareMove();
    }

    private void PreapareMove()
    {
        turnState = MoveState.PlayerPreparingMove;
        foreach (Cell cell in Tower.Cells)
            if (cell.CreatureEntity is EnemyEntity enemy)
                enemiesPreparingMove.Add(enemy);
    }
    private void PreparePlayerMove()
    {
        turnState = MoveState.PlayerMakingMove;
        Tower.Player.PrepareMove();
    }
    private void MakePlayerMove()
    {
        if (Tower.Player.ReadyToMakeMove)
        {
            turnState = MoveState.ForceMove;
            Tower.Player.MakeMove();
        }
    }
    private void FinishPlayerMove()
    {
        if (!Tower.Player.IsAnimated)
        {
            turnState = MoveState.EnemiesPreparingMove;
            Tower.Player.FinishMove();
        }
    }

    private void PrepareEnemiesMove()
    {
        turnState = MoveState.EnemiesMakingMove;
        foreach (CreatureEntity creature in enemiesPreparingMove)
        {
            creature.PrepareMove();
            enemiesMakingMove.Add(creature);
        }
        enemiesPreparingMove.Clear();
    }
    private void MakeEnemiesMove()
    {
        turnState = MoveState.EnemiesFinishingMove;
        foreach (CreatureEntity creature in enemiesMakingMove)
        {
            creature.MakeMove();
            enemiesFinishingMove.Add(creature);
        }
        enemiesMakingMove.Clear();
    }
    private void FinishEnemiesMove()
    {
        ClearEnemiesFinishingMove();
        if (enemiesFinishingMove.Count == 0)
            turnState = MoveState.PreparingMove;
    }
    
    private void FinishForceMoves()
    {
        ClearEnemiesFinishingMove();
        if (enemiesFinishingMove.Count == 0)
            turnState = MoveState.PlayerFinishingMove;
    }

    private void ClearEnemiesFinishingMove()
    {
        for (int i = 0; i < enemiesFinishingMove.Count; i++)
            if (!enemiesFinishingMove[i].IsAnimated)
            {
                enemiesFinishingMove[i].FinishMove();
                enemiesFinishingMove.RemoveAt(i);
                i--;
            }
    }

    public void ForceMove(EnemyEntity enemyEntity)
    {
        if (enemiesPreparingMove.Remove(enemyEntity))
            enemyEntity.PrepareMove();
        if (enemyEntity.State == CreatureEntity.MoveState.MakingMove)
        {
            enemyEntity.MakeMove();
            enemiesFinishingMove.Add(enemyEntity);
        }
    }

    public void DestroyEnemy(EnemyEntity enemyEntity)
    {
        enemiesPreparingMove.Remove(enemyEntity);
        enemiesMakingMove.Remove(enemyEntity);
        enemiesFinishingMove.Remove(enemyEntity);
    }
}