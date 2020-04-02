using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerEntity : CreatureEntity
{
    public WeaponItem defaultWeapon;
    public ArmorItem defaultArmour;

    public InputController InputController { get; private set; }

    public Cell Target { get; private set; }
    public bool ReadyToMakeMove => Target != null && CanInteract(Target);

    public static PlayerEntity Instantiate(GameObject prefab, Cell cell, Text goldText)
    {
        PlayerEntity player = (PlayerEntity)Instantiate(prefab, cell).GetComponent<Entity>();
        player.InputController.Inventory.SetText(goldText);
        return player;
    }

    protected new void Awake()
    {
        base.Awake();

        Camera.main.GetComponent<CameraFollower>().followedObject = transform;

        InputController = FindObjectOfType<InputController>();
        InputController.PlayerEntity = this;
        InputController.Inventory.PlayerEntity = this;
        InputController.AbilityController.SetAbility(ActiveAbility);
    }

    protected void Start()
    {
        defaultWeapon?.Use(this);
        defaultArmour?.Use(this);

        CheckCell();
        Tower.StartLevel();
    }

    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void MoveTo(Cell cell)
    {
        InputController.Inventory.HideDrop();
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
        InputController.Inventory.ShowDrop(Cell.ItemEntities);
    }

    protected override void Attack(CreatureEntity creature)
    {
        base.Attack(creature);
        creature.Cell.Room.AggroEnemies();
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
                TurnController.ForceMove(enemy);
    }

    protected override void Interact(CreatureEntity creature)
    {
        if (creature is EnemyEntity enemy)
            Attack(enemy);
    }

    public void SetTarget(Cell cell)
    {
        if (TurnController.AbleToMakeMove)
            Target = cell;
    }

    public override void MakeMove()
    {
        if (MakeMove(Target))
            base.MakeMove();
    }
    
    public override void FinishMove()
    {
        base.FinishMove();
        CheckCell();
        Target = null;
    }

    public override string GetDescription()
    {
        return "You";
    }
}