using DungeonTower.Entity.Action;

namespace DungeonTower.Entity.EntityActionAIControllers
{
    public interface IEntityActionAIController
    {
        ActionOption GetAction();
    }
}