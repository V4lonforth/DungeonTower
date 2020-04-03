using System.Linq;

public class ShieldBashActivity : ActiveAbility
{
    public StunAbilityEffect stunEffect;

    protected override void Activate(Cell cell)
    {
        Instantiate(stunEffect).ApplyEffect(cell.CreatureEntity);
    }

    public override bool CanTarget(Cell cell)
    {
        return base.CanTarget(cell) && CreatureEntity.Cell.ConnectedCells.Contains(cell) && cell.CreatureEntity is EnemyEntity;
    }
}