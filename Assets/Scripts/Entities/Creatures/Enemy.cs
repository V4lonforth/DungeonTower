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
        Energy.chargeBar.Hide();
        Energy.Rechargable = false;
    }

    public override void Die()
    {
        Tower.Favor.AddFavor(this);
        Destroy();
    }

    public void Aggro()
    {
        if (!aggroed)
        {
            aggroed = true;
            Energy.chargeBar.Show();
            Energy.SetCooldown(wakeUpCooldown);
            Energy.Rechargable = true;
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
        Energy.SetCooldown(movementCooldown);
    }

    protected override void MoveTo(Cell cell)
    {
        base.MoveTo(cell);
        Energy.SetCooldown(movementCooldown);
    }

    protected override void Attack(Creature creature)
    {
        base.Attack(creature);
        Energy.SetCooldown(attackCooldown);
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