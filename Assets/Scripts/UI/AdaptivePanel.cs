using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.UI
{
    public class AdaptivePanel : MonoBehaviour
    {
        private RectTransform rectTransform;
        private readonly List<GameObject> elemenets = new List<GameObject>();

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public GameObject AddElement(GameObject elementPrefab)
        {
            GameObject element = Instantiate(elementPrefab, rectTransform);
            elemenets.Add(element);
            return element;
        }

        public bool RemoveElement(GameObject gameObject)
        {
            if (elemenets.Remove(gameObject))
            {
                Destroy(gameObject);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            foreach (GameObject gameObject in elemenets)
                Destroy(gameObject);
            elemenets.Clear();
        }

        public bool Contains(GameObject gameObject)
        {
            return elemenets.Contains(gameObject);
        }
    }
}
