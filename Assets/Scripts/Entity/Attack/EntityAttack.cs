using DungeonTower.Entity.Action;
using DungeonTower.Entity.Health;
using DungeonTower.Level.Base;
using DungeonTower.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace DungeonTower.Entity.Attack
{
    public class EntityAttack : EntityAction
    {
        [SerializeField] private int damage;
        [SerializeField] private DamageType damageType;

        [SerializeField] private float attackTime = 0.075f;
        [SerializeField] private float attackMovingSpeed = 3f;

        [SerializeField] private Transform animatedObject;

        public Action<Damage> OnPreAttack { get; set; }
        public Action<Damage> OnPostAttack { get; set; }

        public Action<EntityAttack> OnDamageChanged { get; set; }

        public int AttackDamage => damage;

        public override bool CanInteract(Cell cell)
        {
            return base.CanInteract(cell) && cell.FrontEntity != null 
                && cell.FrontEntity.Team != cellEntity.Team && cell.FrontEntity.GameObject.GetComponent<EntityHealth>() != null 
                && stage.Navigator.CheckPath(cellEntity, cellEntity.Cell, cell);
        }

        public override void Interact(Cell cell)
        {
            base.Interact(cell);

            EntityHealth health = cell.FrontEntity.GameObject.GetComponent<EntityHealth>();
            Damage(health);

            StartCoroutine(AnimateAttack(cell, attackTime));
        }

        private Damage GetDamage()
        {
            return new Damage(damage, damageType);
        }

        public void Damage(EntityHealth health)
        {
            Damage damage = GetDamage();
            OnPreAttack?.Invoke(damage);
            health.TakeDamage(damage);
            OnPostAttack?.Invoke(damage);
        }

        protected IEnumerator AnimateAttack(Cell cell, float attackTimeLeft)
        {
            Direction direction = Direction.GetDirection(cellEntity.Cell.StagePosition, cell.StagePosition);
            float time = attackTimeLeft;
            while (time > 0f)
            {
                animatedObject.position += (Vector3)(direction.RotationVector * (attackMovingSpeed * Time.deltaTime));
                time -= Time.deltaTime;
                yield return null;
            }
            time = attackTimeLeft;
            while (time > 0f)
            {
                animatedObject.position -= (Vector3)(direction.RotationVector * (attackMovingSpeed * Time.deltaTime));
                time -= Time.deltaTime;
                yield return null;
            }

            animatedObject.position = cellEntity.transform.position;
            FinishMove(cell);
        }
    }
}
