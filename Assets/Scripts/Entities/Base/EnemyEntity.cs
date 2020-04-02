public class EnemyEntity : CreatureEntity
{
    public int strength;
    private bool aggroed;

    public override void Die()
    {
        TurnController.DestroyEnemy(this);
        Destroy();
        Tower.TowerGenerator.Filler.GenerateGold(Cell);
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
            if (cell.CreatureEntity is PlayerEntity player)
            {
                MakeMove(cell);
                break;
            }
            else
            {
                if (cell.CreatureEntity is EnemyEntity enemy)
                    TurnController.ForceMove(enemy);
                if (!(cell.CreatureEntity is EnemyEntity))
                {
                    MakeMove(cell);
                    break;
                }
            }
        }
    }

    protected override void Interact(CreatureEntity creature)
    {
        Attack(creature);
    }

    public override string GetDescription()
    {
        return "Enemy";
    }
}