using DungeonTower.Effect.Base;
using DungeonTower.Entity.MoveController;
using UnityEngine;

namespace DungeonTower.Effect.Abilities.Stun
{
    public class StunAbilityEffect : Effect<StunAbilityEffectData>
    {
        private readonly MoveController moveController;
        private int turnsLeft;

        public StunAbilityEffect(StunAbilityEffectData data, GameObject target) : base(data, target)
        {
            moveController = target.GetComponent<MoveController>();
            turnsLeft = data.duration;
        }

        public override bool CanApply()
        {
            return moveController != null && data.duration > 0;
        }

        public override void Apply()
        {
            moveController.Sleeping = true;
            moveController.OnMoveFinished += CountTurns;
        }

        public override void Remove()
        {
            moveController.Sleeping = false;
            moveController.OnMoveFinished -= CountTurns;
        }

        private void CountTurns(MoveController moveController)
        {
            turnsLeft -= 1;
            if (turnsLeft <= 0)
            {
                Remove();
            } 
            else
            {
                moveController.Sleeping = true;
            }
        }
    }
}
