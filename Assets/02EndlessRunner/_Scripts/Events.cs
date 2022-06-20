using UnityEngine;
using UnityEngine.SceneManagement;

namespace EndlessRunner
{
    public class Events : MonoBehaviour
    {
        public void ReplayGame()
        {
            SceneManager.LoadScene("EndlessRunnerScene");
            //Time.timeScale = 1;
        }
        public void MainMenuButton()
        {
            SceneManager.LoadScene("EndlessRunnerMenu");
            //Time.timeScale = 1;
        }
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}