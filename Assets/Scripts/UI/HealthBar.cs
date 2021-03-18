using DungeonTower.Entity.Health;
using TMPro;
using UnityEngine;

namespace DungeonTower.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private EntityHealth health;
        [SerializeField] private TextMeshPro text;

        private void Awake()
        {
            health.OnHealthChanged += DisplayHealth;
            DisplayHealth(health);
        }

        private void DisplayHealth(EntityHealth health)
        {
            text.text = health.CurrentHealth.ToString();
        }
    }
}
