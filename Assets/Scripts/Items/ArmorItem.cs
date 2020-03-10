public class ArmorItem : Item
{
    public int maxValue;
    public int value;

    private void Awake()
    {
        value = maxValue;
    }

    public override void Use(PlayerEntity player)
    {
        player.Inventory.Equip(this);
    }

    public override string GetDescription()
    {
        return $"{value} armor";
    }
}