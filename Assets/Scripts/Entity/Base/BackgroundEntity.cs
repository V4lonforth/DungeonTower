using DungeonTower.Level.Base;

namespace DungeonTower.Entity.Base
{
    public class BackgroundEntity : CellEntity
    {
        public override void Attach(Cell cell)
        {
            Cell = cell;
            Cell.AttachToBack(this);
        }

        public override void Detach()
        {
            if (Cell != null)
            {
                Cell.DetachBack(this);
                Cell = null;
            }
        }
    }
}
