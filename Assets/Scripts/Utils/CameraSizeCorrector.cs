using UnityEngine;

namespace DungeonTower.Utils
{
    public class CameraSizeCorrector : MonoBehaviour
    {
        public int cellAmount;

        private void Awake()
        {
            GetComponent<Camera>().orthographicSize = (float)Screen.height * cellAmount / (Screen.width * 2);
        }
    }
}
