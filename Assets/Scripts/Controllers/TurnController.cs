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

    private List<Creature> enemiesPreparingMove;
    private List<Creature> enemiesMakingMove;
    private List<Creature> enemiesFinishingMove;

    private Dictionary<MoveState, Action> turnActions;

    private void Awake()
    {
        enemiesPreparingMove = new List<Creature>();
        enemiesMakingMove = new List<Creature>();
        enemiesFinishingMove = new List<Creature>();

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
            if (cell.Creature is Enemy enemy)
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
        foreach (Creature creature in enemiesPreparingMove)
        {
            creature.PrepareMove();
            enemiesMakingMove.Add(creature);
        }
        enemiesPreparingMove.Clear();
    }
    private void MakeEnemiesMove()
    {
        turnState = MoveState.EnemiesFinishingMove;
        foreach (Creature creature in enemiesMakingMove)
        {
            if (creature.State == Creature.MoveState.MakingMove)
            {
                creature.MakeMove();
                enemiesFinishingMove.Add(creature);
            }
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

    public void ForceMove(Enemy enemy)
    {
        if (enemiesPreparingMove.Remove(enemy))
            enemy.PrepareMove();
        if (enemy.State == Creature.MoveState.MakingMove)
        {
            enemy.MakeMove();
            enemiesFinishingMove.Add(enemy);
        }
    }

    public void DestroyEnemy(Enemy enemy)
    {
        enemiesPreparingMove.Remove(enemy);
        enemiesMakingMove.Remove(enemy);
        enemiesFinishingMove.Remove(enemy);
    }
}