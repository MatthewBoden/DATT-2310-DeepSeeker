using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameStartScene")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LoadMainScene();
            }
        }
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("GameStartScene");
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
