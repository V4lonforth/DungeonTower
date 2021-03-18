using DungeonTower.Entity.Health;
using System;

namespace DungeonTower.Entity.Attack
{
    public interface IAttack
    {
        Action<Damage> OnPreAttack { get; set; }
        Action<Damage> OnPostAttack { get; set; }

        public int AttackDamage { get; }

        void Damage(EntityHealth health);
    }
}
