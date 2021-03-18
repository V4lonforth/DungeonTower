using DungeonTower.Entity.Base;
using UnityEngine;

namespace DungeonTower.Entity.Interactable
{
    public abstract class ExpendableInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private int maxInteractions;
        private int interactionsLeft;

        public bool CanInteract => interactionsLeft > 0;

        protected void Awake()
        {
            interactionsLeft = maxInteractions;
        }

        public void Interact(CellEntity cellEntity)
        {
            if (CanInteract)
            {
                ExpandableInteract(cellEntity);
                interactionsLeft--;
            }
        }

        protected abstract void ExpandableInteract(CellEntity cellEntity);
    }
}
