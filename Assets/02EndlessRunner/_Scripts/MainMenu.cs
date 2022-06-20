using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace EndlessRunner
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI totalCoinText;
        private void Start()
        {
            totalCoinText.text = PlayerPrefs.GetInt("TotalCoins", 0).ToString();
        }
        public void PlayGame()
        {
            SceneManager.LoadScene("EndlessRunnerScene");
        }
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}