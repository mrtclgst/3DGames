using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUiController : MonoBehaviour
{
    [SerializeField] GameObject gameLoseUi, gameWonUi;//uileri gameobje olarak attýk
    bool gameIsOver;

    private void Start()
    {
        //
        Guard.OnGuardHasSpottedPlayer += ShowGameLoseUi;
        FindObjectOfType<Player>().OnReachedFinishPoint += ShowGameWinUi;
    }
    private void Update()
    {
        if (gameIsOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
    void ShowGameWinUi()
    {
        OnGameOver(gameWonUi);
    }
    void ShowGameLoseUi()
    {
        OnGameOver(gameLoseUi);
    }
    void OnGameOver(GameObject gameOverUi)
    {
        gameOverUi.SetActive(true);
        gameIsOver = true;
        Guard.OnGuardHasSpottedPlayer -= ShowGameLoseUi;
        FindObjectOfType<Player>().OnReachedFinishPoint -= ShowGameWinUi;
    }
}
