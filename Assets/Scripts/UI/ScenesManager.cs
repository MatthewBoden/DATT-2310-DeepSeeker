using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;
    [SerializeField] private Animator transitionAnimator;
    
    public GameObject fishContainer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
    
    private IEnumerator LoadScene(string sceneName)
    {
        transitionAnimator?.SetTrigger("Close");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
        transitionAnimator?.SetTrigger("Open");
    }

    // Load the Start Scene
    public void LoadStartScene()
    {
        SceneManager.LoadScene("GameStartScene");
    }

    // Load the Main Game Scene (Level 1)
    public void LoadMainScene()
    {
        Debug.Log("Play again button clicked.");
        StartCoroutine(LoadScene("MainScene"));
    }

    // Load Level 2
    public void LoadLevel2()
    {
        StartCoroutine(LoadScene("Level2"));
    }

    // Load the Game Over Scene for Level 1
    public void LoadGameOverScene()
    {
        StartCoroutine(LoadScene("GameOverScene"));
    }

    // Load the Game Over Scene for Level 2
    public void LoadGameOverScene2()
    {
        StartCoroutine(LoadScene("GameOverScene2"));
    }

    // Load the Win Scene for Level 1
    public void LoadWinScene()
    {
        StartCoroutine(LoadScene("WinScene"));
    }

    // Load the Win Scene for Level 2
    public void LoadWinScene2()
    {
        StartCoroutine(LoadScene("WinScene2"));
    }

    // Load the Game Options Menu
    public void LoadGameOptionsScene()
    {
        StartCoroutine(LoadScene("GameOptionsScene"));
    }

    // Load the How to Play Scene
    public void LoadHowToPlayScene()
    {
        StartCoroutine(LoadScene("HowToPlayScene"));
    }

    // Load the Controls Scene
    public void LoadControlsScene()
    {
        StartCoroutine(LoadScene("ControlsScene"));
    }

    // Load the Credits Scene
    public void LoadCreditsScene()
    {
        StartCoroutine(LoadScene("CreditsScene"));
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
