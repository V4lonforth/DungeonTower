using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerGenerator : MonoBehaviour
{
    public Vector2Int size;

    public Tilemap floorTilemap;
    public Tilemap wallsTilemap;
    public Tilemap fogOfWarTilemap;

    public TileBase fogOfWarTile;

    public List<RoomType> roomTypes;
    public Player playerPrefab;

    public Tower Tower { get; private set; }

    public RoomGenerator RoomGenerator { get; private set; }
    public CellConnector CellConnector { get; private set; }
    public Painter Painter { get; private set; }
    public Spawner Spawner { get; private set; }
    public LootGenerator LootGenerator { get; private set; }
    public Concealer Concealer { get; private set; }

    private void Awake()
    {
        Tower = new Tower(this);
        
        RoomGenerator = new RoomGenerator(this);
        CellConnector = new CellConnector(this);
        Painter = new Painter(this);
        Spawner = new Spawner(this);
        LootGenerator = new LootGenerator(this);
        Concealer = new Concealer(this);
    }

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        RoomGenerator.GenerateRooms();
        CellConnector.ConnectCells();
        Painter.Paint();
        Spawner.Spawn();
        //LootGenerator.GenerateLoot();
        Concealer.ConcealTower();
    }
}