using DungeonTower.Level.Base;
using UnityEngine;

namespace DungeonTower.Level.Generation.Cells
{
    [CreateAssetMenu(fileName = "Data", menuName = "TowerGeneration/CellDecorator", order = 1)]
    public class CellDecorator : ScriptableObject
    {
        [SerializeField] private GameObject backgroundPrefab;

        public void Decorate(Stage stage)
        {
            foreach (Cell cell in stage.Cells)
            {
                cell.Transform = Instantiate(backgroundPrefab).transform;
                cell.Transform.position = cell.WorldPosition;
            }
        }
    }
}
