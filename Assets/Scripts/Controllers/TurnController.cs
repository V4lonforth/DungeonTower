using System.Collections.Generic;

public class TurnController
{
    public bool AbleToMakeMove { get; private set; }
    public bool CurrentlyAnimated => enemiesAnimated.Count > 0;

    private Tower tower;
    private List<EnemyEntity> enemiesMakingMove;
    private List<EnemyEntity> enemiesAnimated;

    public TurnController(Tower tower)
    {
        this.tower = tower;

        enemiesMakingMove = new List<EnemyEntity>();
        enemiesAnimated = new List<EnemyEntity>();
    }

    public void PrepareMove()
    {
        AbleToMakeMove = true;
        foreach (Cell cell in tower.Cells)
        {
            if (cell.Entity is EnemyEntity enemy)
            {
                enemy.PrepareMove();
                enemiesMakingMove.Add(enemy);
            }
        }
        tower.Player.PrepareMove();
    }

    public void MakeMove()
    {
        AbleToMakeMove = false;
    }

    public void FinishPlayerMove()
    {
        for (int i = 0; i < enemiesMakingMove.Count; i++)
            if (enemiesMakingMove[i] != null)
                enemiesMakingMove[i].MakeMove();
        enemiesMakingMove.Clear();
        if (enemiesAnimated.Count == 0)
        {
            FinishMove();
        }
    }

    public void FinishMove()
    {
        tower.Lava.FinishMove();
        PrepareMove();
    }

    public void StopEnemyMakingMove(EnemyEntity enemy)
    {
        enemiesMakingMove.Remove(enemy);
    }
    public void StartEnemyAnimation(EnemyEntity enemy)
    {
        enemiesAnimated.Add(enemy);
    }
    public void FinishEnemyAnimation(EnemyEntity enemy)
    {
        if (!enemiesAnimated.Remove(enemy))
        {
            return;
        }
        if (enemiesAnimated.Count == 0)
        {
            FinishMove();
        }
    }
}