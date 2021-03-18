using UnityEngine;

namespace DungeonTower.Effect.Base
{
    public abstract class Effect<DataType> : IEffect
        where DataType : EffectData  
    {
        protected DataType data;
        protected GameObject target;

        public Effect(DataType data, GameObject target)
        {
            this.data = data;
            this.target = target;
        }

        public abstract void Apply();
        public abstract bool CanApply();
        public abstract void Remove();
    }
}