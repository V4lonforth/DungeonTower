public class Enemy : Creature
{
    public int strength;
    public float wakeUpCooldown;
    private bool aggroed;

    public override void Die()
    {
        Destroy();
        Tower.TowerGenerator.LootGenerator.GenerateGold(Cell);
    }

    public void Aggro()
    {
        if (!aggroed)
        {
            aggroed = true;
            cooldown = wakeUpCooldown;
        }
    }

    protected new void Update()
    {
        if (aggroed)
            base.Update();
    }

    public override void MakeMove()
    {
        foreach (Direction direction in Tower.Navigator.GetDirections(Cell))
        {
            Cell cell = Cell.ConnectedCells[direction];
            if (!(cell.Creature is Enemy))
            {
                MakeMove(cell);
                break;
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