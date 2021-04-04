using DungeonTower.Controllers;
using DungeonTower.Entity.Action;
using DungeonTower.Level.Base;
using System.Collections.Generic;
using DungeonTower.Entity.MoveControllers;
using DungeonTower.Utils;
using UnityEngine;

namespace DungeonTower.UI
{
    public class EntityActionUIController : Singleton<EntityActionUIController>
    {
        [SerializeField] private GameObject interactableHighlightPrefab;
        [SerializeField] private GameObject unavailableHighlightPrefab;
        [SerializeField] private GameObject dangerHighlightPrefab;

        private readonly List<GameObject> selectionHighlights = new List<GameObject>();
        private readonly List<GameObject> dangerHighlights = new List<GameObject>();

        private void Awake()
        {
            GameController.Instance.OnStageStart += StartStage;
        }

        private void StartStage(Stage stage)
        {
            MoveController moveController = stage.PlayerEntity.GetComponent<MoveController>();

            moveController.OnActionSelected += HighlightTargets;
            moveController.OnActionDeselected += RemoveHighlights;
        }

        private void HighlightTargets(MoveController moveController, EntityAction entityAction)
        {
            foreach (Cell cell in entityAction.GetAvailableTargets())
            {
                if (entityAction.CanInteract(cell))
                {
                    selectionHighlights.Add(Instantiate(interactableHighlightPrefab, cell.Transform));
                }
                else
                {
                    selectionHighlights.Add(Instantiate(unavailableHighlightPrefab, cell.Transform));
                }
            }
        }

        private void RemoveHighlights(MoveController moveController, EntityAction entityAction)
        {
            foreach (GameObject highlight in selectionHighlights)
            {
                Destroy(highlight);
            }
            selectionHighlights.Clear();
        }

        public List<GameObject> HighlightDanger(List<Cell> cells)
        {
            List<GameObject> highlights = new List<GameObject>();

            foreach (Cell cell in cells)
            {
                GameObject highlight = Instantiate(dangerHighlightPrefab, cell.Transform);
                highlights.Add(highlight);
                dangerHighlights.Add(highlight);
            }

            return highlights;
        }

        public void RemoveHighlightDanger(List<GameObject> highlights)
        {
            if (highlights == null) return;
            
            foreach (GameObject highlight in highlights)
            {
                dangerHighlights.Remove(highlight);
                Destroy(highlight);
            }
        }
    }
}
