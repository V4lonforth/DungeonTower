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
    }

    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void MoveTo(Cell cell)
    {
        InputController.Inventory.HideDrop();
        CheckWallsTransparency(cell);
        base.MoveTo(cell);
    }

    private void CheckWallsTransparency(Cell cell)
    {
        if (cell.Room != Cell.Room)
        {
            Tower.TowerGenerator.Decorator.SetVisibility(Cell.Room, true);
            Tower.TowerGenerator.Decorator.SetVisibility(cell.Room, false);
        }
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

    protected override void Interact(Creature creature)
    {
        if (creature is Enemy enemy)
            Attack(enemy);
    }

    public void SetTarget(Cell cell)
    {
        Target = cell;
    }

    protected override bool MakeMove(Cell cell)
    {
        if (base.MakeMove(cell))
        {
            cell.Room.AggroEnemies();
            CheckCell();
            return true;
        }
        return false;
    }

    public override void MakeMove()
    {
        if (Target != null)
        {
            MakeMove(Target);
            Target = null;
        }
    }

    public override string GetDescription()
    {
        return "You";
    }
}