using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.UI.ButtonPanels
{
    public class ButtonPanel : MonoBehaviour
    {
        private RectTransform rectTransform;
        public List<GameObject> Elemenets { get; } = new List<GameObject>();

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public virtual GameObject AddElement(GameObject elementPrefab)
        {
            GameObject element = Instantiate(elementPrefab, rectTransform);
            element.transform.SetAsFirstSibling();
            Elemenets.Add(element);
            return element;
        }

        public virtual bool RemoveElement(GameObject gameObject)
        {
            if (Elemenets.Remove(gameObject))
            {
                Destroy(gameObject);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            foreach (GameObject gameObject in Elemenets)
                Destroy(gameObject);
            Elemenets.Clear();
        }

        public bool Contains(GameObject gameObject)
        {
            return Elemenets.Contains(gameObject);
        }
    }
}
