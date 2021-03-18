using UnityEngine;

namespace DungeonTower.Effect.Base
{
    public abstract class EffectData : ScriptableObject
    {
        public void Apply(GameObject target)
        {
            IEffect effect = CreateEffect(target);
            if (effect.CanApply())
                effect.Apply();
        }

        public abstract IEffect CreateEffect(GameObject target);
    }
}
