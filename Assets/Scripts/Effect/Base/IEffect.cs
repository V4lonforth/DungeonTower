namespace DungeonTower.Effect.Base
{
    public interface IEffect
    {
        bool CanApply();
        void Apply();
        void Remove();
    }
}
