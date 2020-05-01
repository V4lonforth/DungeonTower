public class ArmorItem : Item
{
    public HealthBar armorBar;

    public override string GetDescription()
    {
        return "Armor";
    }

    public override void Use(Player player)
    {
        throw new System.NotImplementedException();
    }
}