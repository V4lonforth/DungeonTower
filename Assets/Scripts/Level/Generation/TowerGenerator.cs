using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    public Inspector inspector;

    public Builder Builder { get; private set; }
    public Linker Linker { get; private set; }
    public Connector Connector { get; private set; }
    public Painter Painter { get; private set; }
    public Spawner Spawner { get; private set; }
    public LootGenerator LootGenerator { get; private set; }
    public Concealer Concealer { get; private set; }

    private void Awake()
    {
        Builder = GetComponent<Builder>();
        Linker = GetComponent<Linker>();
        Connector = GetComponent<Connector>();
        Painter = GetComponent<Painter>();
        Spawner = GetComponent<Spawner>();
        LootGenerator = GetComponent<LootGenerator>();
        Concealer = GetComponent<Concealer>();
    }

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        Tower tower = Builder.Build(this);
        FindObjectOfType<InputController>().Tower = tower;
        FindObjectOfType<Lava>().Tower = tower;
        Linker.Link(tower);
        Connector.Connect(tower);
        Painter.Paint(tower);
        Spawner.Spawn(tower);
        LootGenerator.GenerateLoot(tower);
        Concealer.ConcealTower(tower);
    }
}