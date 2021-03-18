using DungeonTower.Inventory;
using DungeonTower.Level.Base;
using DungeonTower.Level.Generation.Cells;
using DungeonTower.Level.Generation.Rooms;
using DungeonTower.Level.Generation.Spawners;
using DungeonTower.Level.Generation.Walls;
using DungeonTower.Level.StageController;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int stageSize;

    [SerializeField] private CreatureSpawner creatureSpawner;
    [SerializeField] private ItemSpawner itemSpawner;
    
    [SerializeField] private ItemController itemController;

    [SerializeField] private CellGenerator cellGenerator;
    [SerializeField] private CellDecorator cellDecorator;

    [SerializeField] private RoomGenerator roomGenerator;
    [SerializeField] private RoomConnector roomConnector;

    [SerializeField] private WallConstructor wallConstructor;
    [SerializeField] private WallDecorator wallDecorator;

    [SerializeField] private FogOfWarController fogOfWarController;

    public Stage Generate()
    {
        Cell[,] cells = cellGenerator.Generate(stageSize);

        Room[] rooms = roomGenerator.Generate(cells);
        RoomConnection[] roomConnections = roomConnector.ConnectRooms(cells, rooms);

        WallController wallController = wallConstructor.Construct(rooms, stageSize, roomConnections);
        Navigator navigator = new Navigator(cells);

        Stage stage = new Stage(stageSize, cells, rooms, navigator, fogOfWarController);

        cellDecorator.Decorate(stage);
        wallDecorator.Decorate(cells, wallController);
        fogOfWarController.ConcealStage(stage);
        fogOfWarController.Initialize();

        creatureSpawner.Spawn(stage);
        itemSpawner.Spawn(stage, itemController);

        return stage;
    }
}