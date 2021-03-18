using DungeonTower.Abilities;
using DungeonTower.Effect.Base;
using DungeonTower.Level.Base;
using UnityEngine;

public class ApplyEffectAbility : Ability
{
    [SerializeField] private EffectData abilityEffectData;

    public override bool CanInteract(Cell cell)
    {
        return base.CanInteract(cell) && cell.FrontEntity != null && stage.Navigator.CheckPath(cellEntity, cellEntity.Cell, cell);
    }

    protected override void Activate(Cell cell)
    {
        IEffect effect = abilityEffectData.CreateEffect(cell.FrontEntity.GameObject);
        if (effect.CanApply())
        {
            effect.Apply();
        }

        FinishMove(cell);
    }
}