public class ArmorItem : EquipmentItem
{
    public HealthBar armor;

    private void Awake()
    {
        armor.Initialize();
    }

    public override string GetDescription()
    {
        return $"{armor.Value} armor";
    }
}