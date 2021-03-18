using DungeonTower.Entity.Base;
using DungeonTower.Level.Base;
using System;
using UnityEngine;

namespace DungeonTower.Entity.Items
{
    [RequireComponent(typeof(CellEntity))]
    public class LootItem : MonoBehaviour
    {
        [SerializeField] private Sprite icon;

        public Sprite Icon => icon;
        public CellEntity CellEntity { get; private set; }
        public CellEntity ParentEntity { get; private set; }
        public Action<LootItem> OnItemDestroy { get; set; }

        private void Awake()
        {
            CellEntity = GetComponent<CellEntity>();
        }

        public void Destroy()
        {
            OnItemDestroy?.Invoke(this);
            Destroy(gameObject);
        }

        public void Drop(Cell cell)
        {
            CellEntity.Attach(cell);
            ParentEntity = null;
        }

        public void Pickup(CellEntity cellEntity)
        {
            CellEntity.Detach();
            ParentEntity = cellEntity;
        }
    }
}
