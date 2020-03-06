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
}