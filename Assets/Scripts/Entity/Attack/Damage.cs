namespace DungeonTower.Entity.Attack
{
    public class Damage
    {
        public int StartDamage { get; set; }
        public int DamageDealt { get; set; }
        public int DamageLeft { get; set; }
        public DamageType Type { get; set; }

        public Damage(int damage, DamageType type)
        {
            StartDamage = DamageLeft = damage;
            Type = type;
        }
    }
}