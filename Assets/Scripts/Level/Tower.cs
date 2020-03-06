using UnityEngine;

public class Tower : MonoBehaviour
{
    public Cell this[Vector2Int pos]
    {
        get => this[pos.y, pos.x];
        set => this[pos.y, pos.x] = value;
    }
    public Cell this[int y, int x]
    {
        get => Cells[y, x];
        set => Cells[y, x] = value;
    }

    public Vector2Int Size { get; set; }
    public Room[] Rooms { get; set; }
    public Cell[,] Cells { get; set; }
    public PlayerEntity Player { get; set; }

    public Filler Filler { get; set; }
    public Concealer Concealer { get; set; }
    public Lava Lava { get; set; }
    public Navigator Navigator { get; set; }

    protected void Awake()
    {
        Lava = GetComponentInChildren<Lava>();
    }

    public void Interact(Vector2Int position)
    {
        if (MathHelper.InRange(position, Size))
            Player.Interact(Cells[position.y, position.x]);
    }

    public void StartTurn()
    {
        foreach (Cell cell in Cells)
        {
            if (cell.Entity is EnemyEntity enemy)
                enemy.StartTurn();
        }
    }

    public void EndTurn()
    {
        foreach (Cell cell in Cells)
        {
            if (cell.Entity is EnemyEntity enemy)
                enemy.EndTurn();
        }
        Lava.EndTurn();

        StartTurn();
    }
}