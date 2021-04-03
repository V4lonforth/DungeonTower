using DungeonTower.Entity.Health;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonTower.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private EntityHealth health;
        [SerializeField] private Transform foregroundBar;

        [SerializeField] private SpriteRenderer foregroundSprite;
        [SerializeField] private SpriteRenderer backgroundSprite;
        
        private void Awake()
        {
            health.OnHealthChanged += DisplayHealth;
        }

        private void DisplayHealth(EntityHealth health)
        {
            SetSpritesActive(health.CurrentHealth < health.MaxHealth);

            float size = health.CurrentHealth / (float)health.MaxHealth;

            foregroundBar.localScale = new Vector3(size, 1f, 1f);
            foregroundBar.localPosition = new Vector3((size - 1f) / 2f, 0f, 0f);
        }

        private void SetSpritesActive(bool active)
        {
            foregroundSprite.enabled = active;
            backgroundSprite.enabled = active;
        }
    }
}
