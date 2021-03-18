using DungeonTower.Entity.Base;

namespace DungeonTower.Entity.Interactable
{
    public interface IInteractable
    {
        bool CanInteract { get; }
        void Interact(CellEntity cellEntity);
    }
}
