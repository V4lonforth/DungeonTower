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
    public AttackEffect attackEffect;

    public float movingTime;
    private float movingTimeLeft;

    private Animator animator;
    
    protected void Awake()
    {
        necklace?.Awake();
        armor?.Awake();
        weapon?.Awake();

        attackEffect = GetComponentInChildren<AttackEffect>();
    }

    protected void Update()
    {
        if (movingTimeLeft > 0f)
        {
            if (Time.deltaTime >= movingTimeLeft)
            {
                movingTimeLeft = 0f;
                transform.position = Cell.transform.position;
            }
            else
            {
                transform.position += (Cell.transform.position - transform.position) * (Time.deltaTime / movingTimeLeft);
                movingTimeLeft -= Time.deltaTime;
            }
        }
    }

    public virtual void Interact(Cell cell)
    {
        FaceCell(cell);
        if (cell.Entity is null)
            Replace(cell);
        else if (cell.Entity is ItemEntity item)
            Interact(item);
        else if (cell.Entity is CreatureEntity creature)
            Interact(creature);
    }

    protected void Interact(CreatureEntity creature)
    {
        attackEffect?.Attack(creature.transform.position, () => weapon.Attack(creature));
    }

    protected void Interact(ItemEntity item)
    {
        if (item is ArmorEntity armor)
            Equip(armor.armor);
        else if (item is WeaponEntity weapon)
            Equip(weapon.weapon);
        else if (item is NecklaceEntity necklace)
            Equip(necklace.necklace);
        Replace(item.Cell);
    }

    protected void Equip(ArmorItem armorItem)
    {
        armor = armorItem;
        armor.text = armorText;
        armor.Awake();
    }
    protected void Equip(WeaponItem weaponItem)
    {
        weapon = weaponItem;
        weapon.text = damageText;
        weapon.Awake();
    }
    protected void Equip(NecklaceItem necklaceItem)
    {
        necklace = necklaceItem;
        necklace.text = healthText;
        necklace.Awake();
    }

    public override void MoveTo(Cell cell)
    {
        cell.Entity = this;
        Cell = cell;
        movingTimeLeft += movingTime;
    }

    public void EndAnimation()
    {
        attackEffect.End();
    }

    protected void FaceCell(Cell cell)
    {
        Direction direction = Cell.GetDirectionToCell(cell);
        if (direction == Direction.Left)
            animatedSprite.flipX = true;
        else if (direction == Direction.Right)
            animatedSprite.flipX = false;
    }

    protected virtual bool CanInteract(Cell cell)
    {
        return Cell.ConnectedCells.Contains(cell) || ReferenceEquals(Cell, cell);
    }

    public abstract void Die();
}