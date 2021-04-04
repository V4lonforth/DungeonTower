using System.Collections.Generic;
using DungeonTower.Entity.Action;
using DungeonTower.Entity.Health;
using DungeonTower.Level.Base;
using UnityEngine;

namespace DungeonTower.Entity.Attack
{
    public class PreciseShotEntityAttack : MultipleTurnEntityAction
    {
        [SerializeField] private int damage;
        [SerializeField] private DamageType damageType;
        
        [SerializeField] private int shotDelay;

        private int turnsToShoot;

        protected override void StartAction()
        {
            turnsToShoot = shotDelay;
        }

        protected override void ContinueAction(Cell target)
        {
            if (turnsToShoot > 0)
            {
                turnsToShoot--;
            }
            else
            {
                Shoot(target);
                FinishAction();
            }
        }

        protected override List<Cell> GetHighlightCells(Cell target)
        {
            return new List<Cell> {target};
        }

        private void Shoot(Cell target)
        {
            if (target.FrontEntity == null) return;
            
            EntityHealth health = target.FrontEntity.GetComponent<EntityHealth>();
            health.TakeDamage(GetDamage());
        }
        
        private Damage GetDamage()
        {
            return new Damage(damage, damageType);
        }
    }
}