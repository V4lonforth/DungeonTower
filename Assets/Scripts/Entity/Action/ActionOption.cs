using DungeonTower.Level.Base;

namespace DungeonTower.Entity.Action
{
    public class ActionOption
    {
        public EntityAction EntityAction { get; }
        public Cell Target { get; }

        public ActionOption(EntityAction entityAction, Cell target)
        {
            EntityAction = entityAction;
            Target = target;
        }
    }
}
