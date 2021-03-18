using DungeonTower.Effect.Base;
using UnityEngine;

namespace DungeonTower.Effect.Weapons.Vampirism
{
    [CreateAssetMenu(fileName = "Data", menuName = "Effects/Weapon/Vampirism", order = 1)]
    public class VampirismWeaponEffectData : EffectData
    {
        public float vampirismPercentage;

        public override IEffect CreateEffect(GameObject target)
        {
            return new VampirismWeaponEffect(this, target);
        }
    }
}
