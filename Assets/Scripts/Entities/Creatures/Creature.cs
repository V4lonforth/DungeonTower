using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : Entity
{
    public SpriteRenderer animatedSprite;

    public Health health;
    public Armor armor;
    public Weapon weapon;

    public List<Effect> Effects { get; private set; }

    private const float MovingTime = 0.1f;

    public static Creature CreateInstance(Creature creaturePrefab, Cell cell)
    {
        Creature creature = (Creature)CreateInstance(creaturePrefab.prefab, cell);
        cell.Entity = creature;
        return creature;
    }

    protected void Awake()
    {
        Effects = new List<Effect>();

        health.Initialize(this);
        armor.Initialize(this);
        weapon.Initialize(this);
    }

    protected bool CanMove(Target target)
    {
        return target.Cell != null && target.Cell.Entity == null && Cell.IsConnected(target.Cell);
    }

    protected void MoveTo(Target target)
    {
        target.Cell.Entity = this;
        Cell.Entity = null;
        Cell = target.Cell;

        StartCoroutine(MoveToParentCell(MovingTime));
    }

    private IEnumerator MoveToParentCell(float movingTimeLeft)
    {
        while (movingTimeLeft > 0f)
        {
            if (Time.deltaTime >= movingTimeLeft)
            {
                movingTimeLeft = 0f;
                transform.position = Cell.WorldPosition;
            }
            else
            {
                transform.position += (Cell.WorldPosition - transform.position) * (Time.deltaTime / movingTimeLeft);
                movingTimeLeft -= Time.deltaTime;
                yield return null;
            }
        }
    }

    protected void FaceCell(Cell cell)
    {
        Direction direction = Cell.GetDirectionToCell(cell);
        if (90f < direction.Angle && direction.Angle < 270f)
            animatedSprite.flipX = true;
        else if (270f < direction.Angle && direction.Angle < 90f)
            animatedSprite.flipX = false;
    }

    public void TakeDamage(Damage damage)
    {
        armor.TakeDamage(damage);
        health.healthBar.TakeDamage(damage);
    }

    public override void Destroy()
    {
        base.Destroy();
        Cell.Entity = null;
    }

    public abstract void Die();
}