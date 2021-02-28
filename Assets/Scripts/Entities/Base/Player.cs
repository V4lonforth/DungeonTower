using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Creature
{
    public WeaponItem defaultWeapon;
    public ArmorItem defaultArmour;

    public InputController InputController { get; private set; }
    public Inventory Inventory { get; private set; }

    public Cell Target { get; private set; }
    public bool ReadyToMakeMove => Target != null && CanInteract(Target);

    public static Player Instantiate(GameObject prefab, Cell cell, Text goldText)
    {
        Player player = (Player)Instantiate(prefab, cell).GetComponent<Creature>();
        player.InputController.Inventory.SetText(goldText);
        return player;
    }

    protected new void Awake()
    {
        base.Awake();

        Camera.main.GetComponent<CameraFollower>().followedObject = transform;

        InputController = FindObjectOfType<InputController>();
        InputController.Player = this;
        InputController.Inventory.Player = this;
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
        InputController.Inventory.ShowDrop(Cell.Items);
    }

    private void CollectGold()
    {
        for (int i = 0; i < Cell.Items.Count; i++)
            if (Cell.Items[i] is GoldItem gold)
            {
                gold.Use(this);
                i--;
            }
    }

    private void CheckNearbyEnemies()
    {
        foreach (Cell cell in Cell.ConnectedCells)
            if (cell && cell.Creature is Enemy enemy)
                TurnController.ForceMove(enemy);
    }

    protected override void Interact(Creature creature)
    {
        if (creature is Enemy enemy)
            Attack(enemy);
    }

    public void SetTarget(Cell cell)
    {
        if (TurnController.AbleToMakeMove)
            Target = cell;
    }

    protected override bool MakeMove(Cell cell)
    {
        if (base.MakeMove(cell))
        {
            cell.Room.AggroEnemies();
            return true;
        }
        return false;
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