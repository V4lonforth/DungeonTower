public class TurnController
{
    public bool AbleToMakeMove { get; private set; }
    public bool CurrentlyAnimated => enemiesAnimated > 0;

    private Tower tower;
    private int enemiesMakingMove;
    private int enemiesAnimated;

    public TurnController(Tower tower)
    {
        this.tower = tower;
    }

    public void PrepareMove()
    {
        AbleToMakeMove = true;
        foreach (Cell cell in tower.Cells)
        {
            if (cell.Entity is EnemyEntity enemy)
            {
                enemy.PrepareMove();
                enemiesMakingMove++;
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
        foreach (Cell cell in tower.Cells)
        {
            if (cell.Entity is EnemyEntity enemy)
                enemy.MakeMove();
        }
    }

    public void FinishEnemyMove()
    {
        enemiesMakingMove--;
        if (enemiesMakingMove == 0)
        {
            FinishMove();
        }
    }

    public void FinishMove()
    {
        tower.Lava.FinishMove();
        PrepareMove();
    }

    public void StartEnemyAnimation()
    {
        enemiesAnimated++;
    }
    public void FinishEnemyAnimation()
    {
        enemiesAnimated--;
    }
}