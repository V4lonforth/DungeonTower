using UnityEngine;

namespace DungeonTower.Utils.Extensions
{
    public static class Vector2Extension
    {
        public static int ManhattanDistance(this Vector2Int vector)
        {
            return Mathf.Abs(vector.x) + Mathf.Abs(vector.y);
        }

        public static float ManhattanDistance(this Vector2 vector)
        {
            return Mathf.Abs(vector.x) + Mathf.Abs(vector.y);
        }
    }
}
