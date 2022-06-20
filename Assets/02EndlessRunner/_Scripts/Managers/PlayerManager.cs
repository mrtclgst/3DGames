using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EndlessRunner
{
    public class PlayerManager : MonoBehaviour
    {
        internal static int collectedCoins;
        internal static bool gameOver, gameStarted;
        [SerializeField] GameObject gameOverPanel, startGameText;
        [SerializeField] TextMeshProUGUI coinText, newRecordText;
        [SerializeField] GameObject[] characterPrefabs;
        private void Awake()
        {
            int index = PlayerPrefs.GetInt("SelectedCharacter");
            GameObject go = Instantiate(characterPrefabs[index], transform.position, Quaternion.identity);
        }
        private void Start()
        {
            Time.timeScale = 1;
            gameOver = false;
            gameStarted = false;
            collectedCoins = 0;
        }
        private void Update()
        {
            if (gameOver)
                GameOver();

            if (SwipeManager.tap)
                StartGame();

            coinText.text = "Coins: " + collectedCoins;

            Debug.Log(PlayerPrefs.GetInt("TotalCoins", 0));
        }

        private void StartGame()
        {
            gameStarted = true;
            startGameText.SetActive(false);
        }

        internal void GameOver()
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);

            if (collectedCoins > PlayerPrefs.GetInt("HighScore", 0))
            {
                newRecordText.text = "New Record: " + collectedCoins;
                PlayerPrefs.SetInt("HighScore", collectedCoins);
            }
            else if (collectedCoins < PlayerPrefs.GetInt("HighScore", 0))
            {
                newRecordText.text = "Score: " + collectedCoins;
            }
        }
    }
}