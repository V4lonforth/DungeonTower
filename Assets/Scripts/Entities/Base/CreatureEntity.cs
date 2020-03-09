using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class CreatureEntity : Entity
{
    public SpriteRenderer animatedSprite;

    public Health Health { get; private set; }
    public Armor Armor { get; private set; }
    public Weapon Weapon { get; private set; }

    private AttackEffect attackEffect;
    private Animator animator;

    private const float MovingTime = 0.1f;

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
        attackEffect?.Attack(creature.transform.position, () => Weapon.Attack(creature));
    }

    protected void FaceCell(Cell cell)
    {
        Direction direction = Cell.GetDirectionToCell(cell);
        if (direction == Direction.Left)
            animatedSprite.flipX = true;
        else if (direction == Direction.Right)
            animatedSprite.flipX = false;
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
        return Cell.ConnectedCells.Contains(cell) || ReferenceEquals(Cell, cell);
    }

    protected void MakeMove(Cell cell)
    {
        if (CanInteract(cell))
        {
            FaceCell(cell);

            if (cell.CreatureEntity is null)
                MoveTo(cell);
            else
                Interact(cell.CreatureEntity);
        }
    }

    protected abstract void Interact(CreatureEntity creature);

    public abstract void MakeMove();
    public abstract void PrepareMove();
    public abstract void FinishMove();
    public abstract void Die();
}