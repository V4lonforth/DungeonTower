using DungeonTower.Effect.Base;
using DungeonTower.Effect.Weapons.Vampirism;
using DungeonTower.Entity.Attack;
using DungeonTower.Entity.Health;
using UnityEngine;

public class VampirismWeaponEffect : Effect<VampirismWeaponEffectData>
{
    private readonly IAttack attack;
    private readonly EntityHealth health;

    public VampirismWeaponEffect(VampirismWeaponEffectData data, GameObject target) : base(data, target)
    {
        attack = target.GetComponent<IAttack>();
        health = target.GetComponent<EntityHealth>();
    }

    public override bool CanApply()
    {
        return attack != null && health != null;
    }

    public override void Apply()
    {
        attack.OnPostAttack += AddVampirism;
    }

    public override void Remove()
    {
        attack.OnPostAttack -= AddVampirism;
    }

    private void AddVampirism(Damage damage)
    {
        health.RestoreHealth(Mathf.CeilToInt(damage.DamageDealt * data.vampirismPercentage));
    }
}