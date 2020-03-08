using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerEntity : CreatureEntity
{
    public Cell Target { get; private set; }

    public Inventory Inventory { get; private set; }
    
    public static PlayerEntity Instantiate(GameObject prefab, Cell cell, TextMeshPro goldText)
    {
        PlayerEntity player = (PlayerEntity)Instantiate(prefab, cell).GetComponent<Entity>();
        player.Inventory.SetText(goldText);
        return player;
    }

    protected new void Awake()
    {
        base.Awake();
        Camera.main.GetComponent<CameraFollower>().followedObject = transform;
        Inventory = GetComponent<Inventory>();
    }

    protected void Start()
    {
        Tower.Concealer.RevealConnectedRooms(Cell);
        Tower.Navigator.CreateMap(Cell);
        Cell.Room.AggroEnemies();
        Tower.StartLevel();
    }

    public void SetTarget(Cell cell)
    {
        Target = cell;
        if (TurnController.AbleToMakeMove)
            MakeMove();
    }

    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void MoveTo(Cell cell)
    {
        CheckNearbyEnemies();
        base.MoveTo(cell);
    }

    protected override IEnumerator MoveToParentCell(float movingTimeLeft)
    {
        while (TurnController.CurrentlyAnimated)
            yield return null;
        yield return base.MoveToParentCell(movingTimeLeft);
    }

    protected override void StopMoving()
    {
        Tower.Concealer.RevealConnectedRooms(Cell);
        Tower.Navigator.CreateMap(Cell);
        Cell.Room.AggroEnemies();
        base.StopMoving();
    }

    private void CheckNearbyEnemies()
    {
        foreach (Cell cell in Cell.ConnectedCells)
            if (cell && cell.Entity is EnemyEntity enemy)
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

    protected override void Interact(ItemEntity item)
    {
        Cell cell = item.Cell;
        item.Item.Use(this);
        MoveTo(cell);
    }

    public override void MakeMove()
    {
        if (Target != null)
        {
            Cell target = Target;
            Target = null;
            MakeMove(target);
        }
    }

    public override void PrepareMove()
    {
        if (Target != null)
            MakeMove();
    }

    public override void FinishMove()
    {
        TurnController.FinishPlayerMove();
    }
}