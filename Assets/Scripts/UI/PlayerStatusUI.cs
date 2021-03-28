using DungeonTower.Controllers;
using DungeonTower.Entity.Attack;
using DungeonTower.Entity.Health;
using DungeonTower.Level.Base;
using TMPro;
using UnityEngine;

namespace DungeonTower.UI
{
    public class PlayerStatusUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI damageText;

        private void Awake()
        {
            GameController.Instance.OnStageStart += StartStage;
        }

        private void StartStage(Stage stage)
        {
            EntityHealth playerHealth = stage.PlayerEntity.GetComponent<EntityHealth>();
            playerHealth.OnHealthChanged += UpdateHealth;

            EntityAttack playerAttack = stage.PlayerEntity.GetComponent<EntityAttack>();
            playerAttack.OnDamageChanged += UpdateDamage;

            UpdateHealth(playerHealth);
            UpdateDamage(playerAttack);
        }

        private void UpdateHealth(EntityHealth entityHealth)
        {
            healthText.text = entityHealth.CurrentHealth.ToString();
        }

        private void UpdateDamage(EntityAttack entityAttack)
        {
            damageText.text = entityAttack.AttackDamage.ToString();
        }
    }
}
