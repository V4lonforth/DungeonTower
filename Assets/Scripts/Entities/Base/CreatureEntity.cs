using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CreatureEntity : Entity
{
    public delegate void MoveEvent(CreatureEntity sender, Cell target);

    public MoveEvent PrepareMoveEvent;
    public MoveEvent MakeMoveEvent;
    public MoveEvent FinishMoveEvent;
    public MoveEvent AttackEvent;

    public SpriteRenderer animatedSprite;

    public Health Health { get; private set; }
    public Armor Armor { get; private set; }
    public Weapon Weapon { get; private set; }

    public ActiveAbility ActiveAbility { get; private set; }

    public ActiveAbility SelectedAbility { get; private set; }

    public List<Effect> Effects { get; private set; }

    private AttackEffect attackEffect;
    private Animator animator;

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
        attackEffect = GetComponentInChildren<AttackEffect>();

        Health = GetComponent<Health>();
        Armor = GetComponent<Armor>();
        Weapon = GetComponent<Weapon>();

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
        AttackEvent?.Invoke(this, creature.Cell);
        StartCoroutine(AttackAnim(Cell.GetDirectionToCell(creature.Cell), AttackTime));
        attackEffect?.Attack(creature.transform.position, () => Weapon.Attack(creature));
    }

    private void FaceCell(Cell cell)
    {
        Direction direction = Cell.GetDirectionToCell(cell);
        if (direction == Direction.Left)
            animatedSprite.flipX = true;
        else if (direction == Direction.Right)
            animatedSprite.flipX = false;
    }

    private void TryOpenDoor(Cell cell)
    {
        if (cell.Room != Room)
        {
            Direction direction = Cell.GetDirectionToCell(cell);
            Cell.Walls[direction].GetComponent<Door>().Open(direction);
        }
    }

    private void StartMakingMove(Cell cell)
    {
        FaceCell(cell);
        TryOpenDoor(cell);
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
        FinishMove();
    }

    protected virtual void StopMoving()
    {
        transform.position = Cell.transform.position;
        FinishMove();
    }

    protected bool CanInteract(Cell cell)
    {
        if (SelectedAbility != null)
            return SelectedAbility.CanTarget(cell);
        return Cell.ConnectedCells.Contains(cell) || ReferenceEquals(Cell, cell);
    }

    public void SelectAbility(ActiveAbility activeAbility)
    {
        SelectedAbility = activeAbility;
        if (!SelectedAbility.targetRequired)
            MakeMove(Cell);
    }
    public void DeselectAbility()
    {
        SelectedAbility = null;
    }

    protected bool MakeMove(Cell cell)
    {
        if (CanInteract(cell))
        {
            StartMakingMove(cell);
            if (SelectedAbility != null)
            {
                SelectedAbility.Use(cell);
                SelectedAbility = null;
            }
            else if (cell.CreatureEntity is null)
                MoveTo(cell);
            else
                Interact(cell.CreatureEntity);
            return true;
        }
        return false;
    }

    protected abstract void Interact(CreatureEntity creature);

    public virtual void MakeMove()
    {
        MakeMoveEvent?.Invoke(this, null);
    }
    public virtual void PrepareMove()
    {
        PrepareMoveEvent?.Invoke(this, null);
    }
    public virtual void FinishMove()
    {
        FinishMoveEvent?.Invoke(this, null);
        ActiveAbility?.FinishMove();
    }
    public abstract void Die();
}