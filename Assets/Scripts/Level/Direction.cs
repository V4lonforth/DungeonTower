using System.Collections.Generic;
using UnityEngine;

public class Direction
{
    public static readonly Direction Right = new Direction(0, 0f, new Vector2Int(1, 0), 4);
    public static readonly Direction TopRight = new Direction(1, 45f, new Vector2Int(1, 1), 5);
    public static readonly Direction Top = new Direction(2, 90f, new Vector2Int(0, 1), 6);
    public static readonly Direction TopLeft = new Direction(3, 135f, new Vector2Int(-1, 1), 7);
    public static readonly Direction Left = new Direction(4, 180f, new Vector2Int(-1, 0), 0);
    public static readonly Direction BottomLeft = new Direction(5, 225f, new Vector2Int(-1, -1), 1);
    public static readonly Direction Bottom = new Direction(6, 270f, new Vector2Int(0, -1), 2);
    public static readonly Direction BottomRight = new Direction(7, 315f, new Vector2Int(1, -1), 3);

    public static readonly Direction[] Values = new Direction[] { Right, TopRight, Top, TopLeft, Left, BottomLeft, Bottom, BottomRight };
    public static readonly Direction[] Straights = new Direction[] { Right, Top, Left, Bottom };
    public static readonly Direction[] Diagonals = new Direction[] { TopRight, TopLeft, BottomLeft, BottomRight };

    public int Value { get; private set; }
    public float Angle { get; private set; }
    public Vector2Int Shift { get; private set; }
    public Vector2 UnitVector { get; private set; }
    public Direction Opposite => Values[oppositeValue];

    private int oppositeValue;

    public const int DirectionsAmount = 8;
    public const int StraightsAmount = 4;
    public const int DiagonalsAmount = 4;

    private Direction(int value, float angle, Vector2Int shift, int opposite)
    {
        Value = value;
        Angle = angle;
        Shift = shift;
        UnitVector = Shift;
        oppositeValue = opposite;
    }

    public Vector2Int ShiftPosition(Vector2Int pos)
    {
        return pos + Shift;
    }

    public static implicit operator Direction(int value)
    {
        return Values[value];
    }
    public static implicit operator int(Direction value)
    {
        return value.Value;
    }
}