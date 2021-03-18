using DungeonTower.Entity.Base;
using DungeonTower.Utils;

namespace DungeonTower.Entity.CellBorder
{
    public interface IInteractableThrough
    {
        void Interact(CellEntity cellEntity, Direction direction);
    }
}
