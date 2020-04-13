using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Creature : Entity
{
    public delegate void MoveEvent(Creature sender, Cell target);
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
    public Energy energy;

    public Direction FacingDirection { get; private set; }

    public ActiveAbility ActiveAbility { get; private set; }
    public ActiveAbility SelectedAbility { get; set; }

    public List<Effect> Effects { get; private set; }

    private AttackAnimation attackEffect;

    private const float MovingTime = 0.1f;
    private const float AttackTime = 0.075f;
    private const float AttackMovingSpeed = 3f;
    
    public static new Creature Instantiate(GameObject prefab, Cell cell)
    {
        Creature creature = Entity.Instantiate(prefab, cell).GetComponent<Creature>();
        cell.Creature = creature;
        return creature;
    }

    protected void Awake()
    {
        attackEffect = GetComponentInChildren<AttackAnimation>();
        energy = GetComponent<Energy>();

        health.Awake();
        armor.Awake(this);
        weapon.Awake(this);

        ActiveAbility = GetComponent<ActiveAbility>();
        GetComponent<PassiveAbility>()?.effect.ApplyEffect(this);
        Effects = new List<Effect>();

        FacingDirection = Direction.Right;
    }

    protected void Update()
    {
        if (!energy.Empty)
            MakeMove();
    }
    
    public override void Destroy()
    {
        base.Destroy();
        Cell.Creature = null;
    }

    protected virtual void MoveTo(Cell cell)
    {
        if (Cell != null && Cell.Creature == this)
            Cell.Creature = null;
        cell.Creature = this;
        Cell = cell;

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
                transform.position += (Cell.WorldPosition - transform.position) * (Time.deltaTime / movingTimeLeft);
                movingTimeLeft -= Time.deltaTime;
                yield return null;
            }
        }
    }

    protected virtual void Attack(Creature creature)
    {
        StartCoroutine(AttackAnim(Cell.GetDirectionToCell(creature.Cell), AttackTime));
        attackEffect?.Attack(creature.transform, () => weapon.Attack(creature));
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
        {
            animatedSprite.flipX = true;
            FacingDirection = Direction.Left;
        }
        else if (direction == Direction.Right)
        {
            animatedSprite.flipX = false;
            FacingDirection = Direction.Right;
        }
    }

    protected IEnumerator AttackAnim(Direction direction, float attackTimeLeft)
    {
        float time = attackTimeLeft;
        while (time > 0f)
        {
            animatedSprite.transform.position += (Vector3)(direction.UnitVector * (AttackMovingSpeed * Time.deltaTime));
            time -= Time.deltaTime;
            yield return null;
        }
        time = attackTimeLeft;
        while (time > 0f)
        {
            animatedSprite.transform.position -= (Vector3)(direction.UnitVector * (AttackMovingSpeed * Time.deltaTime));
            time -= Time.deltaTime;
            yield return null;
        }
        animatedSprite.transform.position = Cell.WorldPosition;
    }

    public void FinishAttackAnimation()
    {
        attackEffect.End();
    }

    protected virtual void StopMoving()
    {
        transform.position = Cell.WorldPosition;
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
            else if (cell.Creature is null)
                MoveTo(cell);
            else
                Interact(cell.Creature);
            return true;
        }
        return false;
    }

    protected abstract void Interact(Creature creature);

    protected abstract void MakeMove();
    public abstract void Die();
}