using DungeonTower.Effect.Base;
using UnityEngine;

namespace DungeonTower.Effect.Abilities.Stun
{
    [CreateAssetMenu(fileName = "Data", menuName = "Effects/Ability/Stun", order = 1)]
    public class StunAbilityEffectData : EffectData
    {
        public int duration;

        public override IEffect CreateEffect(GameObject target)
        {
            return new StunAbilityEffect(this, target);
        }
    }
}
