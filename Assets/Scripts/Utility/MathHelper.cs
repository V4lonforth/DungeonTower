using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    public static bool InRange(Vector2Int value, Vector2Int max)
    {
        return InRange(value, Vector2Int.zero, max);
    }
    public static bool InRange(Vector2Int value, Vector2Int min, Vector2Int max)
    {
        return InRange(value.x, min.x, max.x) && InRange(value.y, min.y, max.y);
    }

    public static bool InRange(int value, int max)
    {
        return InRange(value, 0, max);
    }
    public static bool InRange(int value, int min, int max)
    {
        return min <= value && value < max;
    }

    public static T GetRandomElement<T>(List<T> elements, Func<T, float> weightSelector)
    {
        if (elements.Count == 0)
            return default(T);
        float sumWeight = elements.Sum(weightSelector);
        float randomValue = UnityEngine.Random.Range(0f, 1f);
        float currentWeight = 0f;
        foreach (T element in elements)
        {
            currentWeight += weightSelector(element) / sumWeight;
            if (randomValue <= currentWeight)
                return element;
        }
        return elements.Last();
    }
}