using DungeonTower.Level.Base;

namespace DungeonTower.Entity.Action
{
    public class WaitAction : EntityAction
    {
        public override void Interact(Cell cell)
        {
            base.Interact(cell);
            FinishMove(cell);
        }
    }
}
