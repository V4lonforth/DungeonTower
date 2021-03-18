using DungeonTower.Level.Base;
using UnityEngine;

namespace DungeonTower.Level.Generation.Cells
{
    [CreateAssetMenu(fileName = "Data", menuName = "TowerGeneration/CellGenerator", order = 1)]
    public class CellGenerator : ScriptableObject
    {
        public Cell[,] Generate(Vector2Int stageSize)
        {
            Cell[,] cells = new Cell[stageSize.y, stageSize.x];

            for (int y = 0; y < stageSize.y; y++)
            {
                for (int x = 0; x < stageSize.x; x++)
                {
                    cells[y, x] = new Cell(new Vector2Int(x, y));
                }
            }
            return cells;
        }
    }
}
