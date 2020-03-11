public class ArmorItem : EquipmentItem
{
    public int maxValue;
    public int value;

    private void Awake()
    {
        value = maxValue;
    }

    public override string GetDescription()
    {
        return $"{value} armor";
    }
}