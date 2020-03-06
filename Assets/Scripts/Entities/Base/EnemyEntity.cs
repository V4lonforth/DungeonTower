public class EnemyEntity : CreatureEntity
{
    private bool aggroed;
    private bool moved;

    private float multiplier;

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
    }

    public void StartTurn()
    {
        moved = false;
    }

    public void Aggro()
    {
        if (!aggroed)
        {
            aggroed = true;
            moved = true;
        }
    }

    public override void Interact(Cell cell)
    {
        if (CanInteract(cell))
        {
            if (cell.Entity is ItemEntity)
            {
                FaceCell(cell);
                Swap(cell);
            }
            else
            {
                base.Interact(cell);
            }
        }
    }

    public void EndTurn()
    {
        if (!moved)
        {
            moved = true;
            if (aggroed)
            {
                foreach (Direction direction in Tower.Navigator.GetDirections(Cell))
                {
                    Cell cell = Cell.ConnectedCells[direction];
                    if (cell.Entity is PlayerEntity player)
                        Interact(cell);
                    else
                    {
                        if (cell.Entity is EnemyEntity enemy)
                            enemy.EndTurn();
                        if (!(cell.Entity is EnemyEntity))
                        {
                            Interact(cell);
                            break;
                        }
                    }
                }
            }
        }
    }
}