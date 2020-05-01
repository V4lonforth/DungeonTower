using UnityEngine;

public class Enemy : Creature
{
    public int strength;

    public float timeToWakeUp;

    public float attackCharge;
    public float attackRecharge;

    public float moveCharge;
    public float moveRecharge;

    public ChargeBar chargeBar;

    private EnemyMoveDescription moveDescription;

    private enum State { Waiting, Recharging, Charging }

    private State state;
    private float timer;
    private float timeToRecharge;
    private float timeToCharge;

    private bool aggroed;

    protected void Update()
    {
        timer += Time.deltaTime;
        if (state != State.Waiting)
        {
            if (state == State.Recharging)
            {
                if (timer >= timeToRecharge)
                    Charge();
            }
            if (state == State.Charging)
            {
                if (timer >= timeToCharge + timeToRecharge)
                    MakeMove();
            }
            chargeBar.Fill(timer / (timeToCharge + timeToRecharge));
        }
    }

    public void Aggro()
    {
        if (!aggroed)
        {
            aggroed = true;
            PrepareMove();
        }
    }

    private void MakeMove()
    {
        FaceCell(moveDescription.Target.Cell);
        if (moveDescription.CanTarget(moveDescription.Target))
            moveDescription.MakeMove(moveDescription.Target);
        PrepareMove();
    }

    private void Charge()
    {
        state = State.Charging;
    }

    private void PrepareMove()
    {
        timer = 0f;
        state = State.Recharging;

        if (moveDescription != null)
            timeToRecharge = moveDescription.RechargeTime;
        else
            timeToRecharge = timeToWakeUp;

        moveDescription = FindNextMove();
        timeToCharge = moveDescription.ChargeTime;
    }

    protected virtual EnemyMoveDescription FindNextMove()
    {
        foreach (Direction direction in Tower.Navigator.GetDirections(Cell))
        {
            Cell cell = Cell.ConnectedCells[direction];
            if (cell.Entity == null)
                return new EnemyMoveDescription(attackCharge, attackRecharge, new Target(cell), MoveTo, CanMove);
            else if (cell.Entity is Player)
                return new EnemyMoveDescription(moveCharge, moveRecharge, new Target(cell), weapon.Attack, weapon.CanAttack);
        }
        return new EnemyMoveDescription(0.5f, 0.5f, new Target(Cell), target => { }, target => true);
    }

    public override void Die()
    {
        Destroy();
    }

    public override string GetDescription()
    {
        return "Enemy";
    }
}