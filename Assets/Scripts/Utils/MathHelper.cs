using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.Utils
{
    public static class MathHelper
    {
        public static bool InRange(int value, int min, int max) => min <= value && value < max;
        public static bool InRange(int value, int max) => InRange(value, 0, max);

        public static bool InRange(Vector2Int value, Vector2Int min, Vector2Int max) => InRange(value.x, min.x, max.x) && InRange(value.y, min.y, max.y);
        public static bool InRange(Vector2Int value, Vector2Int max) => InRange(value, Vector2Int.zero, max);

        public static bool InRange<T>(Vector2Int value, T[,] array) => InRange(value, GetArraySize(array));

        public static Vector2Int GetArraySize<T>(T[,] array) => new Vector2Int(array.GetLength(1), array.GetLength(0));

        public static T GetRandomElement<T>(List<T> elements)
        {
            if (elements.Count == 0)
                return default;
            return elements[UnityEngine.Random.Range(0, elements.Count)];
        }
    }
}
