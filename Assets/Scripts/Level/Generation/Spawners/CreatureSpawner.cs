using DungeonTower.Entity.Base;
using DungeonTower.Level.Base;
using DungeonTower.Utils.Weights;
using System.Linq;
using UnityEngine;

namespace DungeonTower.Level.Generation.Spawners
{
    [CreateAssetMenu(fileName = "Data", menuName = "TowerGeneration/CreatureSpawner", order = 1)]
    public class CreatureSpawner : ScriptableObject
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private WeightedList<GameObject> enemyPrefabs;
        [SerializeField] private float spawnChance = 1f;

        public void Spawn(Stage stage)
        {
            foreach (Room room in stage.Rooms.Skip(1))
            {
                foreach (Cell cell in room.Cells)
                {
                    if (Random.Range(0f, 1f) < spawnChance)
                    {
                        GenerateEnemy(enemyPrefabs.GetRandom(), cell);
                    }
                }
            }

            stage.PlayerEntity = GeneratePlayer(stage[0, 0]);
        }

        public CellEntity GenerateEnemy(GameObject prefab, Cell cell)
        {
            CellEntity enemy = Instantiate(prefab).GetComponent<CellEntity>();
            enemy.Attach(cell);
            enemy.transform.position = cell.Transform.position;
            return enemy;
        }

        public CellEntity GeneratePlayer(Cell cell)
        {
            CellEntity player = Instantiate(playerPrefab).GetComponent<CellEntity>();
            player.Attach(cell);
            player.transform.position = cell.Transform.position;
            return player;
        }
    }
}
