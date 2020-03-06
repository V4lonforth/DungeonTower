using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerEntity : CreatureEntity
{
    public int Gold { get; private set; }
    private Text goldText;

    public static PlayerEntity Instantiate(GameObject prefab, Cell cell, Text goldText)
    {
        PlayerEntity player = (PlayerEntity)Instantiate(prefab, cell).GetComponent<Entity>();
        player.goldText = goldText;
        return player;
    }

    protected new void Awake()
    {
        base.Awake();
        Camera.main.GetComponent<CameraFollower>().followedObject = transform;
    }

    protected void Start()
    {
        Tower.Concealer.RevealConnectedRooms(Cell);
        Tower.Navigator.CreateMap(Cell);
        Cell.Room.AggroEnemies();
    }

    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void Replace(Cell cell)
    {
        CheckNearbyEnemies();
        base.Replace(cell);
    }
    public override void Swap(Cell cell)
    {
        CheckNearbyEnemies();
        base.Swap(cell);
    }

    public override void MoveTo(Cell cell)
    {
        base.MoveTo(cell);
        Tower.Concealer.RevealConnectedRooms(cell);
        Tower.Navigator.CreateMap(cell);
        cell.Room.AggroEnemies();
    }

    private void CheckNearbyEnemies()
    {
        foreach (Cell cell in Cell.ConnectedCells)
            if (cell && cell.Entity is EnemyEntity enemy)
                enemy.EndTurn();
    }

    public override void Interact(Cell cell)
    {
        if (CanInteract(cell))
        {
            if (cell.Entity is GoldEntity gold)
            {
                FaceCell(cell);
                Collect(gold.gold);
                Replace(cell);
            }
            else if (!(cell.Entity is PlayerEntity player))
                base.Interact(cell);
            Tower.EndTurn();
        }
    }

    private void Collect(GoldItem gold)
    {
        Gold += gold.Amount;
        goldText.text = Gold.ToString();
    }
}