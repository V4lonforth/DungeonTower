using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.Utils
{
    public class Direction
    {
        public static readonly List<Direction> Values = new List<Direction>(DirectionsAmount);

        public static readonly Direction Right = new Direction(0);
        public static readonly Direction Top = new Direction(1);
        public static readonly Direction Left = new Direction(2);
        public static readonly Direction Bottom = new Direction(3);

        public const int DirectionsAmount = 4;

        private readonly int value;

        private static readonly float[] rotationsFloat = new float[] { 0f, 90f, 180f, 270f };
        private static readonly Vector2[] rotationsVector = new Vector2[] { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
        private static readonly Direction[] opposites = new Direction[] { Left, Bottom, Right, Top };
        private static readonly Vector2Int[] shifts = new Vector2Int[] { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };

        public Direction Opposite => opposites[value];
        public float RotationFloat => rotationsFloat[value];
        public Vector2 RotationVector => rotationsVector[value];
        public Vector2Int ShiftPosition(Vector2Int pos) => pos + shifts[value];

        private Direction(int value)
        {
            this.value = value;
            Values.Add(this);
        }

        public static Direction GetDirection(Vector2Int from, Vector2Int to) => GetDirection(to - from);
        public static Direction GetDirection(Vector2Int offset) => Values.Find(v => v.RotationVector == offset);

        public static implicit operator Direction(int value) => Values[value];
        public static implicit operator int(Direction value) => value.value;
    }
}