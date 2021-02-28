using UnityEngine;

public class Lava : MonoBehaviour
{
    public GameObject lavaPrefab;
    public int startTurnsToRise;
    public int turnsToRise;

    public Linker Linker { get; set; }

    private int turnsToRiseLeft;
    private int currentLevel;

    private Tower tower;

    private void Awake()
    {
        turnsToRiseLeft = startTurnsToRise;
        tower = GetComponentInParent<Tower>();
    }

    public void FinishMove()
    {
        turnsToRiseLeft -= 1;
        if (turnsToRiseLeft <= 0)
        {
            turnsToRiseLeft += turnsToRise;
            CheckCreatures(currentLevel);
            GenerateLavaRow(currentLevel);
            UnlinkRow(currentLevel);
            currentLevel++;
        }
    }

    private void CheckCreatures(int y)
    {
        for (int x = 0; x < tower.Size.x; x++)
            if (tower[y, x].Creature is Creature creature)
                creature.Die();
    }

    private void UnlinkRow(int y)
    {
        for (int x = 0; x < tower.Size.x; x++)
            Linker.UnlinkCell(tower[y, x]);
    }

    private void GenerateLavaRow(int y)
    {
        for (int x = 0; x < tower.Size.x; x++)
            GenerateLava(new Vector2Int(x, y));
    }

    private void GenerateLava(Vector2Int position)
    {
        Instantiate(lavaPrefab, transform).transform.position = (Vector2)position;
    }
}