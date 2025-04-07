using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public GameObject fishContainer;

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainScene" || SceneManager.GetActiveScene().name == "Level2")
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
        if (fishContainer != null && fishContainer.transform.childCount == 0)
        {
            if (SceneManager.GetActiveScene().name == "MainScene") {
                GameManager.instance.SavePlayerData(
                    FindObjectOfType<PlayerController>(),
                    FindObjectOfType<InventoryManager>()
                );
                LoadWinScene();
            }
                
            else if (SceneManager.GetActiveScene().name == "Level2")
                LoadWinScene2();
        }
    }

    // Load the Start Scene
    public void LoadStartScene()
    {
        SceneManager.LoadScene("GameStartScene");
    }

    // Load the Main Game Scene (Level 1)
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    // Load Level 2
    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    // Load the Game Over Scene for Level 1
    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    // Load the Game Over Scene for Level 2
    public void LoadGameOverScene2()
    {
        SceneManager.LoadScene("GameOverScene2");
    }

    // Load the Win Scene for Level 1
    public void LoadWinScene()
    {
        SceneManager.LoadScene("WinScene");
    }

    // Load the Win Scene for Level 2
    public void LoadWinScene2()
    {
        SceneManager.LoadScene("WinScene2");
    }

    // Load the Game Options Menu
    public void LoadGameOptionsScene()
    {
        SceneManager.LoadScene("GameOptionsScene");
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
