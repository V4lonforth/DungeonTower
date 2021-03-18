using DungeonTower.Level.Base;

namespace DungeonTower.Entity.Base
{
    public class ForegroundEntity : CellEntity
    {
        public override void Attach(Cell cell)
        {
            Cell = cell;
            cell.AttachToFront(this);
        }

        public override void Detach()
        {
            if (Cell != null)
            {
                Cell.DetachFront();
                Cell = null;
            }
        }
    }
}
