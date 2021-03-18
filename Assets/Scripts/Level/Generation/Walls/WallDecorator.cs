using DungeonTower.Entity.CellBorder;
using DungeonTower.Level.Base;
using DungeonTower.Level.StageController;
using DungeonTower.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.Level.Generation.Walls
{
    [CreateAssetMenu(fileName = "Data", menuName = "TowerGeneration/WallDecorator", order = 1)]
    public class WallDecorator : ScriptableObject
    {
        [SerializeField] private GameObject horizontalWallPrefab;
        [SerializeField] private GameObject verticalWallPrefab;
        [SerializeField] private GameObject connectorWallPrefab;

        [SerializeField] private GameObject doorPrefab;
        [SerializeField] private GameObject trapdoorPrefab;

        [SerializeField] private GameObject platformPrefab;
        [SerializeField] private GameObject lPlatformPrefab;
        [SerializeField] private GameObject rPlatformPrefab;
        [SerializeField] private GameObject lrPlatformPrefab;

        private Dictionary<int, GameObject> platforms;

        public void Decorate(Cell[,] cells, WallController wallController)
        {
            platforms = new Dictionary<int, GameObject>()
            {
                { 0b00, platformPrefab },
                { 0b01, rPlatformPrefab },
                { 0b10, lPlatformPrefab },
                { 0b11, lrPlatformPrefab },
            };

            DecorateInnerWalls(cells, wallController);
            DecorateOuterWalls(cells);
        }

        private void DecorateInnerWalls(Cell[,] cells, WallController wallController)
        {
            Vector2Int stageSize = MathHelper.GetArraySize(cells);

            for (int y = 0; y < stageSize.y - 1; y++)
            {
                for (int x = 0; x < stageSize.x; x++)
                {
                    switch (wallController.HorizontalWalls[y, x])
                    {
                        case WallType.Wall:
                            InstantiateBorderEntity(horizontalWallPrefab, cells[y, x], cells[y + 1, x]);
                            break;
                        case WallType.Door:
                            InstantiateBorderEntity(trapdoorPrefab, cells[y, x], cells[y + 1, x]);
                            break;
                        case WallType.None:
                            int key = 0;
                            
                            if (x == 0 || wallController.VerticalWalls[y, x - 1] != WallType.None || wallController.VerticalWalls[y + 1, x - 1] != WallType.None)
                                key = 2;

                            if (x == stageSize.x - 1 || wallController.VerticalWalls[y, x] != WallType.None || wallController.VerticalWalls[y + 1, x] != WallType.None)
                                key += 1;

                            if (platforms.TryGetValue(key, out GameObject gameObject))
                                Instantiate(gameObject, cells[y, x].Transform).transform.position += new Vector3(0f, 0.5f);
                            break;
                    }
                }
            }

            for (int y = 0; y < stageSize.y; y++)
            {
                for (int x = 0; x < stageSize.x - 1; x++)
                {
                    switch (wallController.VerticalWalls[y, x])
                    {
                        case WallType.Wall:
                            InstantiateBorderEntity(verticalWallPrefab, cells[y, x], cells[y, x + 1]);
                            break;
                        case WallType.Door:
                            InstantiateBorderEntity(doorPrefab, cells[y, x], cells[y, x + 1]);
                            break;
                    }
                }
            }

            for (int y = 0; y < stageSize.y - 1; y++)
            {
                for (int x = 0; x < stageSize.x - 1; x++)
                {
                    int wallCount = 0;
                    if (wallController.HorizontalWalls[y, x] != WallType.None) wallCount++;
                    if (wallController.HorizontalWalls[y, x + 1] != WallType.None) wallCount++;
                    if (wallController.VerticalWalls[y, x] != WallType.None) wallCount++;
                    if (wallController.VerticalWalls[y + 1, x] != WallType.None) wallCount++;
                    if (wallCount > 1)
                        Instantiate(connectorWallPrefab, cells[y, x].Transform).transform.position += new Vector3(0.5f, 0.5f);
                }
            }
        }

        private GameObject InstantiateBorderEntity(GameObject prefab, Cell first, Cell second)
        {
            BorderEntity borderEntity = Instantiate(prefab, first.Transform).GetComponent<BorderEntity>();
            if (borderEntity != null)
                borderEntity.Attach(first, second);

            borderEntity.transform.position += (Vector3)((second.WorldPosition - first.WorldPosition) / 2f);
            return borderEntity.gameObject;
        }

        private GameObject InstantiateBorderEntity(GameObject prefab, Cell first, Direction direction)
        {
            BorderEntity borderEntity = Instantiate(prefab, first.Transform).GetComponent<BorderEntity>();
            if (borderEntity != null)
                borderEntity.Attach(first, direction);

            borderEntity.transform.position += (Vector3)(direction.RotationVector / 2f);
            return borderEntity.gameObject;
        }

        private void DecorateOuterWalls(Cell[,] cells)
        {
            Vector2Int stageSize = MathHelper.GetArraySize(cells);

            for (int x = 0; x < stageSize.x; x++)
            {
                InstantiateBorderEntity(horizontalWallPrefab, cells[0, x], Direction.Bottom);
                InstantiateBorderEntity(horizontalWallPrefab, cells[stageSize.y - 1, x], Direction.Top);

                Instantiate(connectorWallPrefab, cells[0, x].Transform).transform.position += new Vector3(-0.5f, -0.5f);
                Instantiate(connectorWallPrefab, cells[stageSize.y - 1, x].Transform).transform.position += new Vector3(0.5f, 0.5f);
            }

            for (int y = 0; y < stageSize.y; y++)
            {
                InstantiateBorderEntity(verticalWallPrefab, cells[y, 0], Direction.Left);
                InstantiateBorderEntity(verticalWallPrefab, cells[y, stageSize.x - 1], Direction.Right);

                Instantiate(connectorWallPrefab, cells[y, 0].Transform).transform.position += new Vector3(-0.5f, 0.5f);
                Instantiate(connectorWallPrefab, cells[y, stageSize.x - 1].Transform).transform.position += new Vector3(0.5f, -0.5f);
            }
        }
    }
}
