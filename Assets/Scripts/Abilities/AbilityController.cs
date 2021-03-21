using DungeonTower.Controllers;
using DungeonTower.Entity.Action;
using DungeonTower.Entity.MoveController;
using DungeonTower.Level.Base;
using DungeonTower.UI;
using DungeonTower.UI.ButtonPanels;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.Abilities
{
    public class AbilityController : MonoBehaviour
    {
        [SerializeField] private ButtonPanel abilityPanel;
        [SerializeField] private GameObject abilityButtonPrefab;

        private readonly List<AbilityButton> abilityButtons = new List<AbilityButton>();

        private MoveController playerMoveController;
        private Ability[] abilities;

        private Ability selectedAbility;

        private void Awake()
        {
            GameController.Instance.OnStageStart += StartStage;
        }

        private void StartStage(Stage stage)
        {
            playerMoveController = stage.PlayerEntity.GetComponent<MoveController>();
            playerMoveController.OnActionDeselected += DeselectAbility;

            abilities = stage.PlayerEntity.GetComponents<Ability>();

            foreach (Ability ability in abilities)
            {
                AbilityButton abilityButton = abilityPanel.AddElement(abilityButtonPrefab).GetComponent<AbilityButton>();
                abilityButton.AttachAbility(ability);
                abilityButton.OnSelect += PressAbility;
                abilityButtons.Add(abilityButton);
            }
        }

        private void DeselectAbility(MoveController moveController, EntityAction entityAction)
        {
            selectedAbility = null;
        }

        private void DeselectAbility()
        {
            playerMoveController.DeselectAction();
        }

        private void SelectAbility(Ability ability)
        {
            if (ability.Ready)
            {
                selectedAbility = ability;
                playerMoveController.SelectAction(ability);
            }
        }

        private void PressAbility(Ability ability)
        {
            if (selectedAbility == null)
            {
                SelectAbility(ability);
            }
            else if (selectedAbility == ability)
            {
                DeselectAbility();
            }
            else
            {
                DeselectAbility();
                SelectAbility(ability);
            }
        }
    }
}
