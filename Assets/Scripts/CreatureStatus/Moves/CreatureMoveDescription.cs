using System;

public class CreatureMoveDescription
{
    public Target Target { get; private set; }

    public Action<Target> MakeMove { get; private set; }
    public Func<Target, bool> CanTarget { get; private set; }

    public CreatureMoveDescription(Target target, Action<Target> makeMove, Func<Target, bool> canTarget)
    {
        Target = target;
        MakeMove = makeMove;
        CanTarget = canTarget;
    }
}