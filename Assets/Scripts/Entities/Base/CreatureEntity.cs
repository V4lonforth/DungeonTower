using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CreatureEntity : Entity
{
    public enum MoveState
    {
        PreparingMove,
        MakingMove,
        FinishingMove
    }

    public delegate void MoveEvent(CreatureEntity sender, Cell target);
    public delegate void DamageEvent(Damage damage);

    public MoveEvent PrepareMoveEvent;
    public MoveEvent MakeMoveEvent;
    public MoveEvent FinishMoveEvent;
    public DamageEvent AttackEvent;
    public DamageEvent PostAttackEvent;
    public DamageEvent TakeDamageEvent;

    public SpriteRenderer animatedSprite;

    public Health health;
    public Armor armor;
    public Weapon weapon;

    public ActiveAbility ActiveAbility { get; private set; }
    public ActiveAbility SelectedAbility { get; set; }

    public List<Effect> Effects { get; private set; }

    public MoveState State { get; protected set; }
    public bool IsAnimated { get; protected set; }

    public bool SkipTurn { get; set; }
    protected bool skippedTurn;

    private AttackAnimation attackEffect;

    private const float MovingTime = 0.1f;
    private const float AttackTime = 0.075f;
    private const float AttackMovingSpeed = 3f;

    public new static CreatureEntity Instantiate(GameObject prefab, Cell cell)
    {
        CreatureEntity entity = (CreatureEntity)Entity.Instantiate(prefab, cell);
        cell.CreatureEntity = entity;
        return entity;
    }

    protected void Awake()
    {
        attackEffect = GetComponentInChildren<AttackAnimation>();

        health.Awake();
        armor.Awake(this);
        weapon.Awake(this);
        
        ActiveAbility = GetComponent<ActiveAbility>();
        Effects = new List<Effect>();
    }

    public override void Destroy()
    {
        base.Destroy();
        Cell.CreatureEntity = null;
    }

    public virtual void MoveTo(Cell cell)
    {
        if (Cell != null && Cell.CreatureEntity == this)
            Cell.CreatureEntity = null;
        cell.CreatureEntity = this;
        Cell = cell;

        IsAnimated = true;
        StartCoroutine(MoveToParentCell(MovingTime));
    }

    protected virtual IEnumerator MoveToParentCell(float movingTimeLeft)
    {
        while (movingTimeLeft > 0f)
        {
            if (Time.deltaTime >= movingTimeLeft)
            {
                movingTimeLeft = 0f;
                StopMoving();
            }
            else
            {
                transform.position += (Cell.transform.position - transform.position) * (Time.deltaTime / movingTimeLeft);
                movingTimeLeft -= Time.deltaTime;
                yield return null;
            }
        }
    }

    protected virtual void Attack(CreatureEntity creature)
    {
        IsAnimated = true;
        StartCoroutine(AttackAnim(Cell.GetDirectionToCell(creature.Cell), AttackTime));
        attackEffect?.Attack(creature.transform.position, () => weapon.Attack(creature));
    }

    public void TakeDamage(Damage damage)
    {
        TakeDamageEvent?.Invoke(damage);
        armor.TakeDamage(damage);
        if (armor.Destroyed)
            health.health.TakeDamage(damage);
        if (health.health.Destroyed)
            Die();
    }

    private void FaceCell(Cell cell)
    {
        Direction direction = Cell.GetDirectionToCell(cell);
        if (direction == Direction.Left)
            animatedSprite.flipX = true;
        else if (direction == Direction.Right)
            animatedSprite.flipX = false;
    }

    protected IEnumerator AttackAnim(Direction direction, float attackTimeLeft)
    {
        float time = attackTimeLeft;
        while (time > 0f)
        {
            animatedSprite.transform.position += (Vector3)(direction.Rotation2 * (AttackMovingSpeed * Time.deltaTime));
            time -= Time.deltaTime;
            yield return null;
        }
        time = attackTimeLeft;
        while (time > 0f)
        {
            animatedSprite.transform.position -= (Vector3)(direction.Rotation2 * (AttackMovingSpeed * Time.deltaTime));
            time -= Time.deltaTime;
            yield return null;
        }
        animatedSprite.transform.position = Cell.transform.position;
    }

    public void FinishAttackAnimation()
    {
        attackEffect.End();
        IsAnimated = false;
    }

    protected virtual void StopMoving()
    {
        transform.position = Cell.transform.position;
        IsAnimated = false;
    }

    protected bool CanInteract(Cell cell)
    {
        if (SelectedAbility != null)
            return SelectedAbility.CanTarget(cell);
        return cell != null && Cell.ConnectedCells.Contains(cell) || ReferenceEquals(Cell, cell);
    }

    protected virtual bool MakeMove(Cell cell)
    {
        if (CanInteract(cell))
        {
            FaceCell(cell);
            Cell.OpenDoor(cell);
            if (SelectedAbility != null)
                SelectedAbility.Use(cell);
            else if (cell.CreatureEntity is null)
                MoveTo(cell);
            else
                Interact(cell.CreatureEntity);
            return true;
        }
        return false;
    }

    protected abstract void Interact(CreatureEntity creature);

    public virtual void PrepareMove()
    {
        PrepareMoveEvent?.Invoke(this, null);
        if (SkipTurn)
        {
            SkipTurn = false;
            skippedTurn = true;
        }
        State = MoveState.MakingMove;
    }
    public virtual void MakeMove()
    {
        MakeMoveEvent?.Invoke(this, null);
        State = MoveState.FinishingMove;
    }
    public virtual void FinishMove()
    {
        FinishMoveEvent?.Invoke(this, null);
        ActiveAbility?.FinishMove();
        State = MoveState.PreparingMove;
    }
    public abstract void Die();
}