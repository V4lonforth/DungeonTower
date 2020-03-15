using System.Linq;

public class ShieldBashActivity : ActiveAbility
{
    public StunEffect stunEffect;

    protected override void Activate(Cell cell)
    {
        stunEffect.Clone().AddEffect(cell.CreatureEntity);
    }

    public override bool CanTarget(Cell cell)
    {
        return base.CanTarget(cell) && CreatureEntity.Cell.ConnectedCells.Contains(cell) && cell.CreatureEntity is EnemyEntity;
    }
}