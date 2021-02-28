using System.Linq;

public class ShieldBashActivity : ActiveAbility
{
    public StunAbilityEffect stunEffect;

    protected override void Activate(Cell cell)
    {
        Instantiate(stunEffect).ApplyEffect(cell.Creature);
    }

    public override bool CanTarget(Cell cell)
    {
        return base.CanTarget(cell) && Creature.Cell.ConnectedCells.Contains(cell) && cell.Creature is Enemy;
    }
}