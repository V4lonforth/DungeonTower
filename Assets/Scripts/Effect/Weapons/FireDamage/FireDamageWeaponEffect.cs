using DungeonTower.Effect.Base;
using DungeonTower.Entity.Attack;
using UnityEngine;

namespace DungeonTower.Effect.Weapons.FireDamage
{
    public class FireDamageWeaponEffect : Effect<FireDamageWeaponEffectData>
    {
        private readonly IAttack attack;

        public FireDamageWeaponEffect(FireDamageWeaponEffectData data, GameObject target) : base(data, target)
        {
            attack = target.GetComponent<IAttack>();
        }

        public override bool CanApply()
        {
            return attack != null;
        }

        public override void Apply()
        {
            attack.OnPreAttack += AddFireDamage;
        }

        public override void Remove()
        {
            attack.OnPreAttack -= AddFireDamage;
        }

        private void AddFireDamage(Damage damage)
        {
            damage.DamageLeft += data.fireDamage;
            damage.Type = DamageType.Fire;
        }
    }
}
