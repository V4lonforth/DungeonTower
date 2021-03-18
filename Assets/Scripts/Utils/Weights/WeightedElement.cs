using System;

namespace DungeonTower.Utils.Weights
{
    [Serializable]
    public class WeightedElement<T>
    {
        public T element;
        public float weight = 1f;
    }
}
