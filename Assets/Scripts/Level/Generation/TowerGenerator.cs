using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    public Inspector inspector;

    public InputController InputController { get; private set; }

    public Builder Builder { get; private set; }
    public Linker Linker { get; private set; }
    public Connector Connector { get; private set; }
    public Decorator Decorator { get; private set; }
    public Filler Filler { get; private set; }
    public Concealer Concealer { get; private set; }

    private void Awake()
    {
        InputController = GetComponent<InputController>();

        Builder = GetComponent<Builder>();
        Linker = GetComponent<Linker>();
        Connector = GetComponent<Connector>();
        Decorator = GetComponent<Decorator>();
        Filler = GetComponent<Filler>();
        Concealer = GetComponent<Concealer>();
    }

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        Tower tower = Builder.Build();
        tower.TowerGenerator = this;
        Linker.Link(tower);
        Connector.Connect(tower);
        Decorator.Decorate(tower);
        Filler.Fill(tower);
        Concealer.ConcealTower(tower);
        InputController.Tower = tower;
    }
}