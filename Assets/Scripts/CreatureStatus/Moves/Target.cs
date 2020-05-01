public class Target
{
    public Cell Cell { get; private set; }
    public float Angle { get; private set; }

    public Target(Cell cell)
    {
        Cell = cell;
    }

    public Target(float angle)
    {
        Angle = angle;
    }
}