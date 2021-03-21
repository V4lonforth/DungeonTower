using DungeonTower.Abilities;
using DungeonTower.Input;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonTower.UI
{
    public class AbilityButton : MonoBehaviour
    {
        [SerializeField] private Image abilityImage;

        private TouchHandler touchHandler;
        private RectTransform rectTransform;

        public Ability Ability { get; private set; }

        public Action<Ability> OnSelect { get; set; }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            touchHandler = new TouchHandler(layer: 100, checkHit: CheckHit, onRelease: Release, usingWorldPosition: false);
            touchHandler.Enable();
        }

        public void AttachAbility(Ability ability)
        {
            Ability = ability;
            abilityImage.sprite = ability.Icon;
        }

        public void DetachAbility()
        {
            Ability = null;
            abilityImage.sprite = null;
        }

        private void Release(Vector2 position)
        {
            OnSelect?.Invoke(Ability);
        }

        private bool CheckHit(Vector2 position)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, position);
        }
    }
}
