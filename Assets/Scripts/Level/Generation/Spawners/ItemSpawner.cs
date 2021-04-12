using DungeonTower.Entity.Base;
using DungeonTower.Entity.Interactable;
using DungeonTower.Inventory;
using DungeonTower.Level.Base;
using DungeonTower.Utils.Weights;
using System.Linq;
using UnityEngine;

namespace DungeonTower.Level.Generation.Spawners
{
    [CreateAssetMenu(fileName = "Data", menuName = "TowerGeneration/ItemSpawner", order = 1)]
    public class ItemSpawner : ScriptableObject
    {
        [SerializeField] private WeightedList<GameObject> itemPrefabs;
        [SerializeField] private float spawnChance;

        public void Spawn(Stage stage, ItemController itemController)
        {
            foreach (Room room in stage.Rooms.Skip(1))
            {
                foreach (Cell cell in room.Cells)
                {
                    if (Random.Range(0f, 1f) <= spawnChance)
                    {
                        SpawnItem(itemPrefabs.GetRandom(), cell, itemController);
                    }
                }
            }
        }

        private void SpawnItem(GameObject chestPrefab, Cell cell, ItemController itemController)
        {
            CellEntity chestEntity = Instantiate(chestPrefab, cell.Transform).GetComponent<CellEntity>();
            chestEntity.Attach(cell);
            chestEntity.GetComponent<ChestInteractable>()?.Initialize(itemController);
        }
    }
}
