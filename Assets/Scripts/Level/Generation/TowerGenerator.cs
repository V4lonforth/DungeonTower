using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    private InputController inputController;

    private Builder builder;
    private Linker linker;
    private Connector connector;
    private Decorator decorator;
    private Filler filler;
    private Concealer concealer;

    private void Awake()
    {
        inputController = GetComponent<InputController>();

        builder = GetComponent<Builder>();
        linker = GetComponent<Linker>();
        connector = GetComponent<Connector>();
        decorator = GetComponent<Decorator>();
        filler = GetComponent<Filler>();
        concealer = GetComponent<Concealer>();
    }

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        Tower tower = builder.Build();
        linker.Link(tower);
        connector.Connect(tower);
        decorator.Decorate(tower);
        filler.Fill(tower);
        concealer.ConcealTower(tower);
        inputController.Tower = tower;
    }
}