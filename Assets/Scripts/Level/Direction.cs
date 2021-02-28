using System;
using System.Collections.Generic;
using UnityEngine;

public class Direction
{
    public static readonly List<Direction> Values = new List<Direction>(DirectionsAmount);

    public static readonly Direction Bottom = new Direction(0);
    public static readonly Direction Left = new Direction(1);
    public static readonly Direction Right = new Direction(2);
    public static readonly Direction Top = new Direction(3);

    public const int DirectionsAmount = 4;

    private readonly int value;

    private static Func<Vector2Int, Vector2Int>[] shiftingFunctions = new Func<Vector2Int, Vector2Int>[]
    {
        pos => new Vector2Int(pos.x, pos.y - 1),
        pos => new Vector2Int(pos.x - 1, pos.y),
        pos => new Vector2Int(pos.x + 1, pos.y),
        pos => new Vector2Int(pos.x, pos.y + 1)
    };

    public Direction Opposite => DirectionsAmount - (value + 1);
    public float Rotation
    {
        get
        {
            if (value == Right) return 0f;
            if (value == Top) return 90f;
            if (value == Left) return 180f;
            if (value == Bottom) return 270f;
            return 0f;
        }
    }
    public Vector2 Rotation2
    {
        get
        {
            if (value == Right) return Vector2.right;
            if (value == Top) return Vector2.up;
            if (value == Left) return Vector2.left;
            if (value == Bottom) return Vector2.down;
            return Vector2.right;
        }
    }

    public Direction(int value)
    {
        this.value = value;
        Values.Add(this);
    }

    public static implicit operator Direction(int value)
    {
        return Values[value];
    }
    public static implicit operator int(Direction value)
    {
        return value.value;
    }

    public Vector2Int ShiftPosition(Vector2Int pos)
    {
        return shiftingFunctions[value](pos);
    }
}