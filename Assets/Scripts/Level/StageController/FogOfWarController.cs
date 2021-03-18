using DungeonTower.Controllers;
using DungeonTower.Entity.CellBorder;
using DungeonTower.Entity.Movement;
using DungeonTower.Level.Base;
using DungeonTower.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.Level.StageController
{
    [CreateAssetMenu(fileName = "Data", menuName = "TowerGeneration/FogOfWarController", order = 1)]
    public class FogOfWarController : ScriptableObject
    {
        [SerializeField] private GameObject fogOfWarPrefab;

        [SerializeField] private GameObject fogOfWarVertical;
        [SerializeField] private GameObject fogOfWarHorizontal;
        [SerializeField] private GameObject fogOfWarVerticalConnector;
        [SerializeField] private GameObject fogOfWarHorizontalConnector;
        [SerializeField] private GameObject fogOfWarInnerCorner;
        [SerializeField] private GameObject fogOfWarOuterCorner;

        [NonSerialized] private GameObject[,] fogOfWar;

        [NonSerialized] private Stage stage;

        public void Initialize()
        {
            GameController.Instance.OnStageStart += StartStage;
        }

        private void StartStage(Stage stage)
        {
            stage.PlayerEntity.GetComponent<IMovementController>().OnMovement += CheckPlayerMovement;
            CheckPlayerMovement(stage.PlayerEntity.GetComponent<IMovementController>(), stage.PlayerEntity.Cell, stage.PlayerEntity.Cell);
        }

        private void CheckPlayerMovement(IMovementController movementController, Cell from, Cell to)
        {
            foreach (Direction direction in Direction.Values)
            {
                BorderEntity borderEntity = to.BorderEntities[direction];
                if (borderEntity != null && borderEntity.transparency == Transparency.Peekable)
                {
                    Cell cell = to.Stage.GetCell(to, direction);
                    RevealRoom(cell.Room);
                }
            }
        }

        public void ConcealStage(Stage stage)
        {
            this.stage = stage;

            fogOfWar = new GameObject[stage.Size.y, stage.Size.x];

            ConcealOutside(stage);
            for (int i = 0; i < stage.Rooms.Length; i++)
                ConcealRoom(stage.Rooms[i]);
            RevealRoom(stage.Rooms[0]);
        }

        private void ConcealRoom(Room room)
        {
            foreach (Cell cell in room.Cells)
            {
                fogOfWar[cell.StagePosition.y, cell.StagePosition.x] = Instantiate(fogOfWarPrefab, cell.Transform);
            }
        }

        private bool HasFogOfWar(Vector2Int position)
        {
            return !(MathHelper.InRange(position, stage.Size) && fogOfWar[position.y, position.x] != null);
        }

        private void Flip(GameObject gameObject, bool flipX, bool flipY)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = flipX;
            spriteRenderer.flipY = flipY;
        }

        private void ShapeFogOfWar(Cell cell)
        {
            if (fogOfWar[cell.StagePosition.y, cell.StagePosition.x] == null)
                return;

            Destroy(fogOfWar[cell.StagePosition.y, cell.StagePosition.x]);
            GameObject fogOfWarParent = Instantiate(new GameObject(), cell.Transform);
            fogOfWar[cell.StagePosition.y, cell.StagePosition.x] = fogOfWarParent;

            if (HasFogOfWar(cell.StagePosition + Vector2Int.up))
            {
                Instantiate(fogOfWarHorizontal, fogOfWarParent.transform);
                if (HasFogOfWar(cell.StagePosition + Vector2Int.right))
                    Instantiate(fogOfWarInnerCorner, fogOfWarParent.transform);
                else if (HasFogOfWar(cell.StagePosition + Vector2Int.one))
                    Instantiate(fogOfWarHorizontalConnector, fogOfWarParent.transform);
            }
            else
            {
                if (HasFogOfWar(cell.StagePosition + Vector2Int.one))
                {
                    if (HasFogOfWar(cell.StagePosition + Vector2Int.right))
                        Instantiate(fogOfWarVerticalConnector, fogOfWarParent.transform);
                    else
                        Instantiate(fogOfWarOuterCorner, fogOfWarParent.transform);
                }
            }

            if (HasFogOfWar(cell.StagePosition + Vector2Int.right))
            {
                Instantiate(fogOfWarVertical, fogOfWarParent.transform);
                if (HasFogOfWar(cell.StagePosition + Vector2Int.down))
                    Flip(Instantiate(fogOfWarInnerCorner, fogOfWarParent.transform), false, true);
            }
            else
            {
                if (HasFogOfWar(cell.StagePosition + new Vector2Int(1, -1)))
                {
                    if (!HasFogOfWar(cell.StagePosition + Vector2Int.down))
                        Flip(Instantiate(fogOfWarOuterCorner, fogOfWarParent.transform), false, true);
                    else
                        Flip(Instantiate(fogOfWarHorizontalConnector, fogOfWarParent.transform), false, true);
                }
            }

            if (HasFogOfWar(cell.StagePosition + Vector2Int.down))
            {
                Flip(Instantiate(fogOfWarHorizontal, fogOfWarParent.transform), false, true);
                if (HasFogOfWar(cell.StagePosition + Vector2Int.left))
                    Flip(Instantiate(fogOfWarInnerCorner, fogOfWarParent.transform), true, true);
            }
            else
            {
                if (HasFogOfWar(cell.StagePosition + new Vector2Int(-1, -1)))
                {
                    if (!HasFogOfWar(cell.StagePosition + Vector2Int.left))
                        Flip(Instantiate(fogOfWarOuterCorner, fogOfWarParent.transform), true, true);
                }
            }

            if (HasFogOfWar(cell.StagePosition + Vector2Int.left))
            {
                Flip(Instantiate(fogOfWarVertical, fogOfWarParent.transform), true, false);
                if (HasFogOfWar(cell.StagePosition + Vector2Int.up))
                    Flip(Instantiate(fogOfWarInnerCorner, fogOfWarParent.transform), true, false);
                else if (HasFogOfWar(cell.StagePosition + new Vector2Int(-1, 1)))
                    Flip(Instantiate(fogOfWarVerticalConnector, fogOfWarParent.transform), true, false);
            }
            else
            {
                if (HasFogOfWar(cell.StagePosition + new Vector2Int(-1, 1)))
                {
                    if (!HasFogOfWar(cell.StagePosition + Vector2Int.up))
                        Flip(Instantiate(fogOfWarOuterCorner, fogOfWarParent.transform), true, false);
                }
            }
        }

        public void RevealRoom(Room room)
        {
            List<Cell> revealingCells = new List<Cell>(room.Cells);

            //for (int i = 0; i < 2; i++)
            //{
            //    List<Cell> nextCells = new List<Cell>();
            //    foreach (Cell cell in revealingCells)
            //    {
            //        foreach (Direction direction in Direction.Values)
            //        {
            //            Cell adjacentCell = stage.GetCell(cell, direction);
            //            if (adjacentCell != null && !revealingCells.Contains(adjacentCell) && !nextCells.Contains(adjacentCell))
            //                nextCells.Add(adjacentCell);
            //        }
            //    }
            //    revealingCells.AddRange(nextCells);
            //}

            foreach (Cell cell in revealingCells)
                ShapeFogOfWar(cell);
        }

        private void ConcealOutside(Stage stage)
        {
            for (Vector2Int position = new Vector2Int(-1, -1); position.x <= stage.Size.x; position.x++)
                Instantiate(fogOfWarPrefab, (Vector2)position, Quaternion.identity);
            for (Vector2Int position = new Vector2Int(-1, stage.Size.y); position.x <= stage.Size.x; position.x++)
                Instantiate(fogOfWarPrefab, (Vector2)position, Quaternion.identity);
            for (Vector2Int position = new Vector2Int(-1, 0); position.y <= stage.Size.y; position.y++)
                Instantiate(fogOfWarPrefab, (Vector2)position, Quaternion.identity);
            for (Vector2Int position = new Vector2Int(stage.Size.x, 0); position.y <= stage.Size.y; position.y++)
                Instantiate(fogOfWarPrefab, (Vector2)position, Quaternion.identity);
        }
    }
}