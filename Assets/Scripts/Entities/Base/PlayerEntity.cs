using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerEntity : CreatureEntity
{
    public Cell Target { get; set; }

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
        Tower.StartLevel();
    }

    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void MoveTo(Cell cell)
    {
        CheckNearbyEnemies();
        base.MoveTo(cell);
        Tower.Concealer.RevealConnectedRooms(cell);
        Tower.Navigator.CreateMap(cell);
        cell.Room.AggroEnemies();
    }

    private void CheckNearbyEnemies()
    {
        foreach (Cell cell in Cell.ConnectedCells)
            if (cell && cell.Entity is EnemyEntity enemy)
                enemy.MakeMove();
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
        if (item is ArmorEntity armor)
            Equip(armor.armor);
        else if (item is WeaponEntity weapon)
            Equip(weapon.weapon);
        else if (item is NecklaceEntity necklace)
            Equip(necklace.necklace);
        else if (item is GoldEntity gold)
            Collect(gold.gold);
        Replace(item.Cell);
    }

    private void Equip(ArmorItem armorItem)
    {
        armor = armorItem;
        armor.text = armorText;
        armor.Awake();
    }
    private void Equip(WeaponItem weaponItem)
    {
        weapon = weaponItem;
        weapon.text = damageText;
        weapon.Awake();
    }
    private void Equip(NecklaceItem necklaceItem)
    {
        necklace = necklaceItem;
        necklace.text = healthText;
        necklace.Awake();
    }

    private void Collect(GoldItem gold)
    {
        Gold += gold.Amount;
        goldText.text = Gold.ToString();
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