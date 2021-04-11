using DungeonTower.Controllers;
using UnityEngine;

namespace DungeonTower.Menu
{
    public class GameOverMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverMenu;
        
        private void Awake()
        {
            gameOverMenu.SetActive(false);
            GameController.Instance.OnGameOver += ShowGameOverMenu;
        }

        private void ShowGameOverMenu()
        {
            gameOverMenu.SetActive(true);
        }
    }
}