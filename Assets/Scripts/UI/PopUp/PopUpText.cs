using System.Collections;
using TMPro;
using UnityEngine;

namespace DungeonTower.UI.PopUp
{
    public class PopUpText : MonoBehaviour
    {
        [SerializeField] private float animationTime;
        [SerializeField] private Vector2 direction;
        [SerializeField] private float acceleration;

        private TextMeshPro text;
        private Vector2 currentVelocity;

        private void Awake()
        {
            text = GetComponent<TextMeshPro>();
        }

        public void Spawn(int value)
        {
            text.text = value.ToString();

            StartCoroutine(AnimateText());
        }

        private IEnumerator AnimateText()
        {
            float currentTime = 0f;
            while (currentTime <= animationTime)
            {
                currentTime += Time.deltaTime;
                Fade(Mathf.Clamp01(currentTime / animationTime));
                Move();
                yield return null;
            }
            Destroy(gameObject);
        }

        private void Fade(float time)
        {
            Color color = text.color;
            color.a = 1f - time;
            text.color = color;
        }

        private void Move()
        {
            currentVelocity += direction * (acceleration * Time.deltaTime);
            transform.position += (Vector3)(currentVelocity * Time.deltaTime);
        }
    }
}
