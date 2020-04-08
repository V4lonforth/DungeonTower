public class Enemy : Creature
{
    public int strength;
    private bool aggroed;

    public override void Die()
    {
        TurnController.DestroyEnemy(this);
        Destroy();
        Tower.TowerGenerator.LootGenerator.GenerateGold(Cell);
    }

    public void Aggro()
    {
        if (!aggroed)
        {
            aggroed = true;
            SkipTurn = true;
        }
    }

    public override void MakeMove()
    {
        base.MakeMove();
        if (!aggroed)
            return;
        if (skippedTurn)
        {
            skippedTurn = false;
            return;
        }

        foreach (Direction direction in Tower.Navigator.GetDirections(Cell))
        {
            Cell cell = Cell.ConnectedCells[direction];
            if (cell.Creature is Player player)
            {
                MakeMove(cell);
                break;
            }
            else
            {
                if (cell.Creature is Enemy enemy)
                    TurnController.ForceMove(enemy);
                if (!(cell.Creature is Enemy))
                {
                    MakeMove(cell);
                    break;
                }
            }
        }
    }

    protected override void Interact(Creature creature)
    {
        Attack(creature);
    }

    public override string GetDescription()
    {
        return "Enemy";
    }
}