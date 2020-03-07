using System.Linq;
using UnityEngine;
using TMPro;

public abstract class CreatureEntity : Entity
{
    public NecklaceItem necklace;
    public ArmorItem armor;
    public WeaponItem weapon;

    public TextMeshPro healthText;
    public TextMeshPro damageText;
    public TextMeshPro armorText;

    public SpriteRenderer animatedSprite;

    private AttackEffect attackEffect;
    private Animator animator;
    
    protected void Awake()
    {
        necklace?.Awake();
        armor?.Awake();
        weapon?.Awake();

        attackEffect = GetComponentInChildren<AttackEffect>();
    }

    protected void Attack(CreatureEntity creature)
    {
        attackEffect?.Attack(creature.transform.position, () => weapon.Attack(creature));
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

    protected override void StopMoving()
    {
        base.StopMoving();
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

            if (cell.Entity is null)
                MoveTo(cell);
            else if (cell.Entity is ItemEntity item)
                Interact(item);
            else if (cell.Entity is CreatureEntity creature)
                Interact(creature);
        }
    }

    protected abstract void Interact(ItemEntity item);
    protected abstract void Interact(CreatureEntity creature);

    public abstract void MakeMove();
    public abstract void PrepareMove();
    public abstract void FinishMove();
    public abstract void Die();
}