using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    public Cell Cell { get; protected set; }
    public Room Room => Cell.Room;
    public Tower Tower => Room.Tower;
    
    public delegate void MoveEvent(Creature sender, Cell target);
    public delegate void DamageEvent(Damage damage);

    public MoveEvent PrepareMoveEvent;
    public MoveEvent MakeMoveEvent;
    public MoveEvent FinishMoveEvent;
    public DamageEvent AttackEvent;
    public DamageEvent PostAttackEvent;
    public DamageEvent TakeDamageEvent;

    public float movementCooldown;

    public SpriteRenderer animatedSprite;

    public Health health;
    public Armor armor;
    public Weapon weapon;
    
    public Direction FacingDirection { get; private set; }

    public ActiveAbility ActiveAbility { get; private set; }
    public ActiveAbility SelectedAbility { get; set; }

    public List<Effect> Effects { get; private set; }

    protected float cooldown;

    private AttackAnimation attackEffect;

    private const float MovingTime = 0.1f;
    private const float AttackTime = 0.075f;
    private const float AttackMovingSpeed = 3f;
    
    public static Creature Instantiate(GameObject prefab, Cell cell)
    {
        Creature creature = Instantiate(prefab, cell.transform).GetComponent<Creature>();
        creature.Cell = cell;
        cell.Creature = creature;
        return creature;
    }

    protected void Awake()
    {
        attackEffect = GetComponentInChildren<AttackAnimation>();

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
        if (cooldown > 0)
            cooldown -= Time.deltaTime;
        else
            MakeMove();
    }

    public void Destroy()
    {
        Destroy(gameObject);
        Cell.Creature = null;
    }

    public virtual void MoveTo(Cell cell)
    {
        cooldown = movementCooldown;
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
                transform.position += (Cell.transform.position - transform.position) * (Time.deltaTime / movingTimeLeft);
                movingTimeLeft -= Time.deltaTime;
                yield return null;
            }
        }
    }

    protected virtual void Attack(Creature creature)
    {
        StartCoroutine(AttackAnim(Cell.GetDirectionToCell(creature.Cell), AttackTime));
        attackEffect?.Attack(creature.transform.position, () => weapon.Attack(creature));
        cooldown = weapon.weaponItem.cooldown;
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
    }

    protected virtual void StopMoving()
    {
        transform.position = Cell.transform.position;
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

    public abstract void MakeMove();
    public abstract string GetDescription();
    public abstract void Die();
}