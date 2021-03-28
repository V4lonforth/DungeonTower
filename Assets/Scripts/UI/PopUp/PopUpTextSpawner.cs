using DungeonTower.Entity.Attack;
using DungeonTower.Entity.Health;
using UnityEngine;

namespace DungeonTower.UI.PopUp
{
    public class PopUpTextSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject damageTextPrefab;
        [SerializeField] private GameObject healTextPrefab;

        [SerializeField] private Vector2 spawnPosition;

        private void Awake()
        {
            EntityHealth entityHealth = GetComponent<EntityHealth>();
            entityHealth.OnPostDamageTaken += ShowDamage;
            entityHealth.OnHeal += ShowHeal;
        }

        private void ShowDamage(EntityHealth entityHealth, Damage damage)
        {
            SpawnText(damageTextPrefab, damage.StartDamage);
        }

        private void ShowHeal(EntityHealth entityHealth, int heal)
        {
            SpawnText(healTextPrefab, heal);
        }

        private void SpawnText(GameObject textPrefab, int value)
        {
            GameObject textGameObject = Instantiate(textPrefab);
            textGameObject.transform.position = transform.position + (Vector3)spawnPosition;

            PopUpText text = textGameObject.GetComponent<PopUpText>();
            text.Spawn(value);
        }
    }
}
