using DungeonTower.Entity.Action;
using DungeonTower.Level.Base;
using System;
using System.Collections.Generic;
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
            return lootItem.ParentEntity != null ? Targeting.CanTarget(lootItem.ParentEntity, cell) : base.CanInteract(cell);
        }

        public override List<Cell> GetAvailableTargets()
        {
            return lootItem.ParentEntity != null ? Targeting.GetAvailableTargets(lootItem.ParentEntity) : base.GetAvailableTargets();
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
