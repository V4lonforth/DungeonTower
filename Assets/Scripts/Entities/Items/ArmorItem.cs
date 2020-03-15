public class ArmorItem : EquipmentItem
{
    public int maxArmor;
    public int armor;

    private void Awake()
    {
        armor = maxArmor;
    }

    public override string GetDescription()
    {
        return $"{armor} armor";
    }
}