using DungeonTower.Effect.Base;
using UnityEngine;

namespace DungeonTower.Effect.Weapons.FireDamage
{
    [CreateAssetMenu(fileName = "Data", menuName = "Effects/Weapon/FireDamage", order = 1)]
    public class FireDamageWeaponEffectData : EffectData
    {
        public int fireDamage;

        public override IEffect CreateEffect(GameObject target)
        {
            return new FireDamageWeaponEffect(this, target);
        }
    }
}
