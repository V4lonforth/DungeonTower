public class WeaponItem : Item
{
    public Damage damage;

    public ProgressBar durability;
    public int degradeSpeed;

    public override string GetDescription()
    {
        return "Weapon";
    }

    public override void Use(Player player)
    {
        throw new System.NotImplementedException();
    }
}