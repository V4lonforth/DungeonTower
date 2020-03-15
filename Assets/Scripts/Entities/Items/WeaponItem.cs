public class WeaponItem : EquipmentItem
{
    public int damage;

    public override string GetDescription()
    {
        return $"{damage} damage";
    }
}