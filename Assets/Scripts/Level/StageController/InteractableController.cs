using DungeonTower.Controllers;
using DungeonTower.Entity.Action;
using DungeonTower.Entity.Base;
using DungeonTower.Entity.Interactable;
using DungeonTower.Entity.MoveController;
using DungeonTower.Entity.Movement;
using DungeonTower.Level.Base;
using DungeonTower.TargetingSystem;
using DungeonTower.UI.ButtonPanels;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonTower.Level.StageController
{
    [RequireComponent(typeof(ButtonPanel))]
    public class InteractableController : MonoBehaviour
    {
        [SerializeField] private GameObject interactableButtonPrefab;
        [SerializeField] private GameObject itemUseButtonPrefab;

        private ButtonPanel interactPanel;
        private GameObject useItemButton;

        private CellEntity playerEntity;
        private MoveController playerMoveController;

        private void Awake()
        {
            interactPanel = GetComponent<ButtonPanel>();
            GameController.Instance.OnStageStart += StartStage;
        }

        private void StartStage(Stage stage)
        {
            playerEntity = stage.PlayerEntity;
            playerMoveController = playerEntity.GetComponent<MoveController>();
            stage.PlayerEntity.GetComponent<IMovementController>().OnMovement += CheckInteractiables;
            playerMoveController.OnActionSelected += ShowUseItemButton;
            playerMoveController.OnActionDeselected += HideUseItemButton;
        }

        private void ClearInteractables()
        {
            interactPanel.Clear();
        }

        private void CheckInteractiables(IMovementController movementController, Cell from, Cell to)
        {
            ClearInteractables();
            ShowInteractables(to);
        }

        private void ShowInteractables(Cell cell)
        {
            foreach (BackgroundEntity backgroundEntity in cell.BackEntities)
            {
                IInteractable interactable = backgroundEntity.GetComponent<IInteractable>();
                if (interactable != null && interactable.CanInteract)
                {
                    Button button = interactPanel.AddElement(interactableButtonPrefab).GetComponent<Button>();
                    button.onClick.AddListener(() => Interact(interactable));
                }
            }
        }

        private void ShowUseItemButton(MoveController moveController, EntityAction entityAction)
        {
            if (entityAction.Targeting is SelfTargeting)
            {
                useItemButton = interactPanel.AddElement(itemUseButtonPrefab);
                Button button = useItemButton.GetComponent<Button>();
                button.onClick.AddListener(() => UseItem(entityAction));
            }
        }

        private void HideUseItemButton(MoveController moveController, EntityAction entityAction)
        {
            if (useItemButton != null)
            {
                interactPanel.RemoveElement(useItemButton);
                useItemButton = null;
            }
        }

        private void Interact(IInteractable interactable)
        {
            interactable.Interact(playerEntity);

            ClearInteractables();
            ShowInteractables(playerEntity.Cell);
        }

        private void UseItem(EntityAction entityAction)
        {
            playerMoveController.SelectMove(new ActionOption(entityAction, playerEntity.Cell));

            ClearInteractables();
            ShowInteractables(playerEntity.Cell);
        }

        public void Interact()
        {
            if (interactPanel.Elemenets.Count > 0)
            {
                interactPanel.Elemenets.Last().GetComponent<Button>().onClick.Invoke();
            }
        }

        public bool CheckDropItemPosition(Vector2 position)
        {
            if (useItemButton != null)
            {
                RectTransform rectTransform = useItemButton.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, position);
                }
            }
            return false;
        }
    }
}
