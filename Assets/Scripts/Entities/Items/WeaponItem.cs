public class WeaponItem : EquipmentItem
{
    public int damage;
    public float cooldown;
    
    public override string GetDescription()
    {
        return $"{damage} damage";
    }
}