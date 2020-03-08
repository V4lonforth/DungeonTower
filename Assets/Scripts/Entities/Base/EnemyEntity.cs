public class EnemyEntity : CreatureEntity
{
    private bool aggroed;
    private bool moved;

    private float multiplier;
    private bool isAnimated;

    public void SetMultiplier(float multiplier)
    {
        this.multiplier = multiplier;
        necklace.SetMultiplier(multiplier);
        weapon.SetMultiplier(multiplier);
        armor?.SetMultiplier(multiplier);
    }

    public override void Die()
    {
        Destroy();
        Cell.Room.Tower.Filler.GenerateGold(Cell);
        FinishMove();
    }

    public override void PrepareMove()
    {
        moved = false;
    }

    public void Aggro()
    {
        if (!aggroed)
        {
            aggroed = true;
            moved = true;
            FinishMove();
        }
    }

    public override void MakeMove()
    {
        if (!moved)
        {
            moved = true;
            isAnimated = true;
            TurnController.StartEnemyAnimation();
            if (aggroed)
            {
                foreach (Direction direction in Tower.Navigator.GetDirections(Cell))
                {
                    bool madeMove = false;
                    Cell cell = Cell.ConnectedCells[direction];
                    if (cell.Entity is PlayerEntity player)
                    {
                        MakeMove(cell);
                        madeMove = true;
                        break;
                    }
                    else
                    {
                        if (cell.Entity is EnemyEntity enemy)
                            enemy.MakeMove();
                        if (!(cell.Entity is EnemyEntity))
                        {
                            MakeMove(cell);
                            madeMove = true;
                            break;
                        }
                    }
                    if (!madeMove)
                        FinishMove();
                }
            }
            else
                FinishMove();
        }
    }

    protected override void Interact(ItemEntity item)
    {
        FaceCell(item.Cell);
        Swap(item.Cell);
    }

    protected override void Interact(CreatureEntity creature)
    {
        Attack(creature);
    }

    public override void FinishMove()
    {
        TurnController.FinishEnemyMove();
        if (isAnimated)
        {
            TurnController.FinishEnemyAnimation();
            isAnimated = false;
        }
    }
}