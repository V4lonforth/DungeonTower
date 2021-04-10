using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class Settings : MonoBehaviour
    {
        public void BackToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}