public class ItemEntity : Entity
{
    public Item Item { get; private set; }

    private void Awake()
    {
        Item = GetComponent<Item>();
        Item.ItemEntity = this;
    }
}