using DungeonTower.Entity.Attack;
using System;
using UnityEngine;

namespace DungeonTower.Entity.Health
{
    public class EntityHealth : MonoBehaviour
    {
        [SerializeField] private int maxHealth;

        public int MaxHealth => maxHealth;
        public int CurrentHealth { get; private set; }

        public Action<Damage, EntityHealth> OnPreDamageTaken { get; set; }
        public Action<Damage, EntityHealth> OnPostDamageTaken { get; set; }
        public Action<EntityHealth> OnHealthChanged { get; set; }
        public Action<EntityHealth> OnDeath { get; set; }

        private void Awake()
        {
            CurrentHealth = maxHealth;
        }

        public void RestoreHealth(int health)
        {
            CurrentHealth += health;
            if (CurrentHealth > maxHealth)
                CurrentHealth = maxHealth;
            OnHealthChanged?.Invoke(this);
        }

        private void Die()
        {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }

        public void TakeDamage(Damage damage)
        {
            OnPreDamageTaken?.Invoke(damage, this);

            if (damage.DamageLeft > 0)
            {
                CurrentHealth -= damage.DamageLeft;
                if (CurrentHealth <= 0)
                {
                    damage.DamageDealt += damage.DamageLeft + CurrentHealth;
                    damage.DamageLeft = -CurrentHealth;
                    CurrentHealth = 0;

                    OnPostDamageTaken?.Invoke(damage, this);
                    OnHealthChanged?.Invoke(this);
                    Die();
                }
                else
                {
                    damage.DamageDealt += damage.DamageLeft;
                    damage.DamageLeft = 0;

                    OnPostDamageTaken?.Invoke(damage, this);
                    OnHealthChanged?.Invoke(this);
                }
            }
        }
    }
}
