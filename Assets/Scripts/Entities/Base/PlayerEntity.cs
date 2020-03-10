using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerEntity : CreatureEntity
{
    public WeaponItem defaultWeapon;
    public ArmorItem defaultArmour;

    public Inventory Inventory { get; private set; }

    public Cell Target { get; private set; }

    public static PlayerEntity Instantiate(GameObject prefab, Cell cell, Text goldText)
    {
        PlayerEntity player = (PlayerEntity)Instantiate(prefab, cell).GetComponent<Entity>();
        player.Inventory.SetText(goldText);
        return player;
    }

    protected new void Awake()
    {
        base.Awake();

        Camera.main.GetComponent<CameraFollower>().followedObject = transform;
        Inventory = FindObjectOfType<Inventory>();
        Inventory.PlayerEntity = this;

        defaultWeapon?.Use(this);
        defaultArmour?.Use(this);
    }

    protected void Start()
    {
        CheckCell();
        Tower.StartLevel();
    }

    public void SetTarget(Cell cell)
    {
        if (TurnController.AbleToMakeMove)
        {
            Target = cell;
            MakeMove();
        }
    }

    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void MoveTo(Cell cell)
    {
        Inventory.HideDrop();
        CheckNearbyEnemies();
        base.MoveTo(cell);
    }

    protected override IEnumerator MoveToParentCell(float movingTimeLeft)
    {
        while (TurnController.CurrentlyAnimated)
            yield return null;
        yield return base.MoveToParentCell(movingTimeLeft);
    }

    private void CheckCell()
    {
        Tower.TowerGenerator.Concealer.RevealConnectedRooms(Cell);
        Tower.Navigator.CreateMap(Cell);
        Cell.Room.AggroEnemies();
        CollectGold();
        Inventory.ShowDrop(Cell.ItemEntities);
    }

    protected override void StopMoving()
    {
        CheckCell();
        base.StopMoving();
    }

    private void CollectGold()
    {
        for (int i = 0; i < Cell.ItemEntities.Count; i++)
            if (Cell.ItemEntities[i].Item is GoldItem gold)
            {
                gold.Use(this);
                i--;
            }
    }

    private void CheckNearbyEnemies()
    {
        foreach (Cell cell in Cell.ConnectedCells)
            if (cell && cell.CreatureEntity is EnemyEntity enemy)
                enemy.MakeMove();
    }

    protected override void Attack(CreatureEntity creature)
    {
        base.Attack(creature);
        creature.Cell.Room.AggroEnemies();
    }

    protected override void Interact(CreatureEntity creature)
    {
        if (creature is EnemyEntity enemy)
            Attack(enemy);
        else
            FinishMove();
    }

    public override void MakeMove()
    {
        if (Target != null)
        {
            Cell target = Target;
            Target = null;
            if (CanInteract(target))
            {
                TurnController.MakeMove();
                MakeMove(target);
            }
        }
    }

    public override void PrepareMove()
    {
        MakeMove();
    }

    public override void FinishMove()
    {
        TurnController.FinishPlayerMove();
    }

    public override string GetDescription()
    {
        return "You";
    }
}