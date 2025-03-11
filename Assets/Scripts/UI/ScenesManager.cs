using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public GameObject fishContainer;

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            CheckWinCondition();
        }

        if (SceneManager.GetActiveScene().name == "GameStartScene" && Input.GetKeyDown(KeyCode.Space))
        {
            LoadMainScene();
        }
    }

    public void CheckWinCondition()
    {
        if (fishContainer.transform.childCount == 0) 
        {
            LoadWinScene();
        }
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("GameStartScene");
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void LoadWinScene()
    {
        SceneManager.LoadScene("WinScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
