using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        public void NewGame()
        {
            SceneManager.LoadScene("NewLevel");
        }

        public void OpenSettings()
        {
            SceneManager.LoadScene("Settings");
        }

        public void Exit()
        {
            Debug.Log("Exit...");
            Application.Quit();
        }
    }
}