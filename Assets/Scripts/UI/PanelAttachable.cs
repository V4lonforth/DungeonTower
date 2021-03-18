using UnityEngine;

namespace DungeonTower.UI
{
    public class PanelAttachable : MonoBehaviour
    {
        [SerializeField] private RectTransform targetRectTransform;
        
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            rectTransform.anchoredPosition = -targetRectTransform.sizeDelta.x * Vector2.right + targetRectTransform.anchoredPosition;
        }
    }
}
