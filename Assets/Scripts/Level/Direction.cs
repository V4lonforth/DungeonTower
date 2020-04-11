using UnityEngine;

public class Direction
{
    public static readonly Direction Right = new Direction(0, 45f, new Vector2Int(1, 0), new Vector2(1f, 1f), 4);
    public static readonly Direction TopRight = new Direction(1, 90f, new Vector2Int(1, 1), new Vector2(0f, 1f), 5);
    public static readonly Direction Top = new Direction(2, 135f, new Vector2Int(0, 1), new Vector2(-1f, 1f), 6);
    public static readonly Direction TopLeft = new Direction(3, 180f, new Vector2Int(-1, 1), new Vector2(-1f, 0f), 7);
    public static readonly Direction Left = new Direction(4, 225f, new Vector2Int(-1, 0), new Vector2(-1f, -1f), 0);
    public static readonly Direction BottomLeft = new Direction(5, 270f, new Vector2Int(-1, -1), new Vector2(0f, -1f), 1);
    public static readonly Direction Bottom = new Direction(6, 315f, new Vector2Int(0, -1), new Vector2(1f, -1f), 2);
    public static readonly Direction BottomRight = new Direction(7, 0f, new Vector2Int(1, -1), new Vector2(1f, 0f), 3);

    public static readonly Direction[] Values = new Direction[] { Right, TopRight, Top, TopLeft, Left, BottomLeft, Bottom, BottomRight };
    public static readonly Direction[] Straights = new Direction[] { Right, Top, Left, Bottom };
    public static readonly Direction[] Diagonals = new Direction[] { TopRight, TopLeft, BottomLeft, BottomRight };

    public int Value { get; private set; }
    public float Angle { get; private set; }
    public Vector2Int Shift { get; private set; }
    public Vector2 UnitVector { get; private set; }
    public Direction Opposite => Values[oppositeValue];
    public Direction Clockwise => Values[(Value - 1 + DirectionsAmount) % DirectionsAmount];
    public Direction Counterclockwise => Values[(Value + 1) % DirectionsAmount];

    private int oppositeValue;

    public const int DirectionsAmount = 8;
    public const int StraightsAmount = 4;
    public const int DiagonalsAmount = 4;

    private Direction(int value, float angle, Vector2Int shift, Vector2 unitVector, int opposite)
    {
        Value = value;
        Angle = angle;
        Shift = shift;
        UnitVector = unitVector;
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