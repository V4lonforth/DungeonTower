﻿public class EnemyEntity : CreatureEntity
{
    private bool aggroed;
    private bool moved;

    private float multiplier;
    private bool isAnimated;

    public override void Die()
    {
        FinishMove();
        TurnController.StopEnemyMakingMove(this);
        Destroy();
        Tower.TowerGenerator.Filler.GenerateGold(Cell);
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
            if (aggroed)
            {
                foreach (Direction direction in Tower.Navigator.GetDirections(Cell))
                {
                    bool madeMove = false;
                    Cell cell = Cell.ConnectedCells[direction];
                    if (cell.CreatureEntity is PlayerEntity player)
                    {
                        MakeMove(cell);
                        madeMove = true;
                        break;
                    }
                    else
                    {
                        if (cell.CreatureEntity is EnemyEntity enemy)
                            enemy.MakeMove();
                        if (!(cell.CreatureEntity is EnemyEntity))
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

    public override void MoveTo(Cell cell)
    {
        isAnimated = true;
        TurnController.StartEnemyAnimation(this);
        base.MoveTo(cell);
    }

    protected override void Attack(CreatureEntity creature)
    {
        isAnimated = true;
        TurnController.StartEnemyAnimation(this);
        base.Attack(creature);
    }

    protected override void Interact(CreatureEntity creature)
    {
        Attack(creature);
    }

    public override void FinishMove()
    {
        if (isAnimated)
        {
            TurnController.FinishEnemyAnimation(this);
            isAnimated = false;
        }
    }

    public override string GetDescription()
    {
        return "Enemy";
    }
}