public class WeaponItem : EquipmentItem
{
    public int value;

    public override string GetDescription()
    {
        return $"{value} damage";
    }
}