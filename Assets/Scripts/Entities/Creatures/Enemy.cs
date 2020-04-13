public class Enemy : Creature
{
    public int strength;

    public float wakeUpCooldown;
    public float movementCooldown;
    public float attackCooldown;

    private bool aggroed;

    protected new void Awake()
    {
        base.Awake();
        energy.chargeBar.Hide();
        energy.Rechargable = false;
    }

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
            energy.chargeBar.Show();
            energy.SetCooldown(wakeUpCooldown);
            energy.Rechargable = true;
        }
    }

    protected new void Update()
    {
        if (aggroed)
            base.Update();
    }

    protected override void MakeMove()
    {
        foreach (Direction direction in Tower.Navigator.GetDirections(Cell))
        {
            Cell cell = Cell.ConnectedCells[direction];
            if (!(cell.Creature is Enemy))
            {
                MakeMove(cell);
                return;
            }
        }
        energy.SetCooldown(movementCooldown);
    }

    protected override void MoveTo(Cell cell)
    {
        base.MoveTo(cell);
        energy.SetCooldown(movementCooldown);
    }

    protected override void Attack(Creature creature)
    {
        base.Attack(creature);
        energy.SetCooldown(attackCooldown);
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