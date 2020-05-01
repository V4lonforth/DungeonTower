using System;

public class EnemyMoveDescription : CreatureMoveDescription
{
    public float ChargeTime { get; private set; }
    public float RechargeTime { get; private set; }

    public EnemyMoveDescription(float chargeTime, float rechargeTime, Target target, Action<Target> makeMove, Func<Target, bool> canTarget) 
        : base(target, makeMove, canTarget)
    {
        ChargeTime = chargeTime;
        RechargeTime = rechargeTime;
    }
}