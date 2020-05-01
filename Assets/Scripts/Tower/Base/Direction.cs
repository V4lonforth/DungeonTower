using UnityEngine;

public class Direction
{
    public int Value { get; private set; }
    public float Angle { get; private set; }
    public Vector2Int Shift { get; private set; }
    public Vector2 RadiusVector { get; private set; }
    public Direction Opposite => Values[oppositeValue];
    public Direction Clockwise => Values[(Value - 1 + DirectionsAmount) % DirectionsAmount];
    public Direction Counterclockwise => Values[(Value + 1) % DirectionsAmount];
    private int oppositeValue;

    public static float CellAngle = Mathf.Atan(CellRatio) * Mathf.Rad2Deg;

    public static readonly Direction Right = new Direction(0, 0f, new Vector2Int(1, -1), new Vector2(1f, 0f), 4);
    public static readonly Direction TopRight = new Direction(1, Right.Angle + CellAngle, new Vector2Int(1, 0), new Vector2(1f, 1f), 5);
    public static readonly Direction Top = new Direction(2, 90f, new Vector2Int(1, 1), new Vector2(0f, 1f), 6);
    public static readonly Direction TopLeft = new Direction(3, Top.Angle + 90f - CellAngle, new Vector2Int(0, 1), new Vector2(-1f, 1f), 7);
    public static readonly Direction Left = new Direction(4, 180f, new Vector2Int(-1, 1), new Vector2(-1f, 0f), 0);
    public static readonly Direction BottomLeft = new Direction(5, Left.Angle + CellAngle, new Vector2Int(-1, 0), new Vector2(-1f, -1f), 1);
    public static readonly Direction Bottom = new Direction(6, 270f, new Vector2Int(-1, -1), new Vector2(0f, -1f), 2);
    public static readonly Direction BottomRight = new Direction(7, Bottom.Angle + 90f - CellAngle, new Vector2Int(0, -1), new Vector2(1f, -1f), 3);

    public static readonly Direction[] Values = new Direction[] { Right, TopRight, Top, TopLeft, Left, BottomLeft, Bottom, BottomRight };
    public static readonly Direction[] Straights = new Direction[] { TopRight, TopLeft, BottomLeft, BottomRight };
    public static readonly Direction[] Diagonals = new Direction[] { Right, Top, Left, Bottom };

    public const float CellRatio = 0.625f;
    public const int DirectionsAmount = 8;

    private Direction(int value, float angle, Vector2Int shift, Vector2 unitVector, int opposite)
    {
        Value = value;
        Angle = angle;
        Shift = shift;
        RadiusVector = unitVector;
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