using UnityEngine;
using UnityEngine.Tilemaps;

public class Lava : MonoBehaviour
{
    public Tilemap lavaTilemap;
    public TileBase lavaTile;
    public float startTimeToRise;
    public float timeToRise;

    private float timeToRiseLeft;
    private int currentLevel;

    public Tower Tower { get; set; }

    private void Awake()
    {
        timeToRiseLeft = startTimeToRise;
    }

    private void Update()
    {
        timeToRiseLeft -= Time.deltaTime;
        if (timeToRiseLeft <= 0f)
        {
            timeToRiseLeft += timeToRise;
            CheckCreatures(currentLevel);
            Tower.TowerGenerator.Painter.RemoveRow(currentLevel);
            Tower.TowerGenerator.Linker.RemoveRow(currentLevel);
            GenerateLavaRow(currentLevel);
            currentLevel++;
        }
    }

    private void CheckCreatures(int x)
    {
        for (int y = 0; y < Tower.Size.y; y++)
        {
            if (Tower[y, x].Creature != null)
                Tower[y, x].Creature.Destroy();
            while (Tower[y, x].Items.Count > 0)
                Tower[y, x].Items[0].Destroy();
        }
    }

    private void GenerateLavaRow(int x)
    {
        for (int y = 0; y < Tower.Size.y; y++)
            lavaTilemap.SetTile(new Vector3Int(x, y, 0), lavaTile);
    }
}