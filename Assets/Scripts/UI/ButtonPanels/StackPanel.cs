using System.Linq;
using UnityEngine;

namespace DungeonTower.UI.ButtonPanels
{
    public class StackPanel : ButtonPanel
    {
        public override GameObject AddElement(GameObject elementPrefab)
        {
            Elemenets.ForEach(e => e.SetActive(false));
            return base.AddElement(elementPrefab);
        }

        public override bool RemoveElement(GameObject gameObject)
        {
            if (base.RemoveElement(gameObject))
            {
                if (Elemenets.Count > 0)
                {
                    Elemenets.Last().SetActive(true);
                }
                return true;
            }
            return false;
        }
    }
}
