using DungeonTower.Entity.Action;
using DungeonTower.Level.Base;
using System;
using UnityEngine;

namespace DungeonTower.Entity.Items
{
    [RequireComponent(typeof(LootItem))]
    public abstract class Consumable : EntityAction
    {
        [SerializeField] private int maxUses;
        private int usesLeft;

        private LootItem lootItem;

        public Action<Consumable, Cell> OnUse { get; set; }

        protected new void Awake()
        {
            base.Awake();
            lootItem = GetComponent<LootItem>();
        }

        public override bool CanInteract(Cell cell)
        {
            if (lootItem.ParentEntity != null)
                return targeting.CanTarget(lootItem.ParentEntity.Cell, cell);
            return base.CanInteract(cell);
        }

        public override void Interact(Cell cell)
        {
            base.Interact(cell);
            OnUse?.Invoke(this, cell);
            Use(cell);

            usesLeft--;
            if (usesLeft <= 0)
            {
                DeleteConsumable();
            }

            FinishMove(cell);
        }

        private void DeleteConsumable()
        {
            if (lootItem != null)
                lootItem.Destroy();
        }

        protected abstract void Use(Cell target);
    }
}
