using DungeonTower.Controllers;
using DungeonTower.Entity.Base;
using DungeonTower.Level.Base;
using DungeonTower.TargetingSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DungeonTower.Entity.Action
{
    [RequireComponent(typeof(CellEntity))]
    public abstract class EntityAction : MonoBehaviour
    {
        [SerializeField] private int controllerPriority;
        [SerializeField] private bool requiredSelection;
        [SerializeField] private Targeting targeting;

        protected CellEntity cellEntity;
        protected Stage stage;

        public Action<EntityAction> OnMoveStart { get; set; }
        public Action<EntityAction> OnMoveFinish { get; set; }

        public int Priority => controllerPriority;
        public bool Selected { get; private set; }
        public bool RequiredSelection => requiredSelection;
        public Targeting Targeting => targeting;

        protected void Awake()
        {
            cellEntity = GetComponent<CellEntity>();
            GameController.Instance.OnStageStart += t => stage = t;
        }

        public virtual bool CanInteract(Cell cell)
        {
            return targeting.CanTarget(cellEntity, cell);
        }

        public virtual List<Cell> GetAvailableTargets()
        {
            return targeting.GetAvailableTargets(cellEntity);
        }

        public virtual void Interact(Cell cell)
        {
            OnMoveStart?.Invoke(this);
        }

        protected virtual void FinishMove(Cell cell)
        {
            OnMoveFinish?.Invoke(this);
        }
    }
}
