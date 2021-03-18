using DungeonTower.Entity.Base;
using DungeonTower.Utils;

namespace DungeonTower.Entity.CellBorder
{
    public interface IPassable
    {
        bool CanPass(CellEntity cellEntity);
        void Pass(CellEntity cellEntity, Direction direction);
    }
}
