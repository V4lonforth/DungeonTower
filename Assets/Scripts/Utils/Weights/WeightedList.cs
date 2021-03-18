using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.Utils.Weights
{
    [Serializable]
    public class WeightedList<T>
    {
        [SerializeField] private List<WeightedElement<T>> weightedElements;

        public T GetRandom()
        {
            if (weightedElements.Count == 0)
                return default;

            float sumWeight = weightedElements.Sum(w => w.weight);
            float randomValue = UnityEngine.Random.Range(0f, 1f);
            float currentWeight = 0f;
            foreach (WeightedElement<T> element in weightedElements)
            {
                currentWeight += element.weight / sumWeight;
                if (randomValue <= currentWeight)
                    return element.element;
            }
            return weightedElements.Last().element;
        }
    }
}
