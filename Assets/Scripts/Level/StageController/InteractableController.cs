using DungeonTower.Controllers;
using DungeonTower.Entity.Base;
using DungeonTower.Entity.Interactable;
using DungeonTower.Entity.Movement;
using DungeonTower.Level.Base;
using DungeonTower.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonTower.Level.StageController
{
    [RequireComponent(typeof(AdaptivePanel))]
    public class InteractableController : MonoBehaviour
    {
        [SerializeField] private GameObject elementPrefab;

        private AdaptivePanel adaptivePanel;
        private CellEntity playerEntity;

        private void Awake()
        {
            adaptivePanel = GetComponent<AdaptivePanel>();
            GameController.Instance.OnStageStart += StartStage;
        }

        private void StartStage(Stage stage)
        {
            playerEntity = stage.PlayerEntity;
            stage.PlayerEntity.GetComponent<IMovementController>().OnMovement += CheckInteractiables;
        }

        private void ClearInteractables()
        {
            adaptivePanel.Clear();
        }

        private void CheckInteractiables(IMovementController movementController, Cell from, Cell to)
        {
            RefreshInteractables(to);
        }

        private void RefreshInteractables(Cell cell)
        {
            ClearInteractables();
            foreach (BackgroundEntity backgroundEntity in cell.BackEntities)
            {
                IInteractable interactable = backgroundEntity.GetComponent<IInteractable>();
                if (interactable != null && interactable.CanInteract)
                {
                    Button button = adaptivePanel.AddElement(elementPrefab).GetComponent<Button>();
                    button.onClick.AddListener(() => Interact(interactable));
                }
            }
        }

        private void Interact(IInteractable interactable)
        {
            interactable.Interact(playerEntity);
            RefreshInteractables(playerEntity.Cell);
        }
    }
}
