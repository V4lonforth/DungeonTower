using DungeonTower.Controllers;
using DungeonTower.Entity.Action;
using DungeonTower.Entity.MoveController;
using DungeonTower.Level.Base;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.UI
{
    public class EntityActionUIController : MonoBehaviour
    {
        [SerializeField] private GameObject interactableHighlightPrefab;
        [SerializeField] private GameObject unavailableHighlightPrefab;

        private readonly List<GameObject> highlights = new List<GameObject>();

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
                    highlights.Add(Instantiate(interactableHighlightPrefab, cell.Transform));
                }
                else
                {
                    highlights.Add(Instantiate(unavailableHighlightPrefab, cell.Transform));
                }
            }
        }

        private void RemoveHighlights(MoveController moveController, EntityAction entityAction)
        {
            foreach (GameObject highlight in highlights)
            {
                Destroy(highlight);
            }
            highlights.Clear();
        }
    }
}
