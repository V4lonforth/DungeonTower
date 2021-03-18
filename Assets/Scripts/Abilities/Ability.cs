using DungeonTower.Entity.Action;
using DungeonTower.Entity.MoveController;
using DungeonTower.Level.Base;
using UnityEngine;

namespace DungeonTower.Abilities
{
    public abstract class Ability : EntityAction
    {
        [SerializeField] private int cooldown;
        [SerializeField] private Sprite icon;

        private int turnsToRecharge;

        public Sprite Icon => icon;
        public bool Ready => turnsToRecharge <= 0;

        protected new void Awake()
        {
            base.Awake();

            GetComponent<MoveController>().OnMoveStarted += Recharge;
        }

        public override bool CanInteract(Cell cell)
        {
            return Ready && base.CanInteract(cell);
        }

        public override void Interact(Cell cell)
        {
            base.Interact(cell);
            turnsToRecharge = cooldown;
            Activate(cell);
        }

        private void Recharge(MoveController moveController)
        {
            if (turnsToRecharge > 0)
                turnsToRecharge--;
        }

        protected abstract void Activate(Cell cell);
    }
}
