using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }

    private static readonly int AnimatorParamEnter = Animator.StringToHash("Enter");
    private static readonly int AnimatorParamExit = Animator.StringToHash("Exit");

    public GameObject fishContainer;
    [SerializeField] private Animator transitionAnimator;

    private bool _isPlaying;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fishContainer = GameObject.Find("FishContainer");
        _isPlaying = true;
    }
    
    void Update()
    {
        if ((SceneManager.GetActiveScene().name == "MainScene" || SceneManager.GetActiveScene().name == "Level2") && _isPlaying)
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
            if (SceneManager.GetActiveScene().name == "MainScene")
            {
                GameManager.instance.SavePlayerData(
                    FindObjectOfType<PlayerController>(),
                    FindObjectOfType<InventoryManager>()
                );
                LoadWinScene();
                _isPlaying = false;
            }
            else if (SceneManager.GetActiveScene().name == "Level2")
            {
                LoadWinScene2();
                _isPlaying = false;
            }
        }
    }

    private IEnumerator LoadScene(string sceneName)
    {
        transitionAnimator.SetTrigger(AnimatorParamExit);
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(sceneName);
        transitionAnimator.SetTrigger(AnimatorParamEnter);
    }

    // Load the Start Scene
    public void LoadStartScene()
    {
        StartCoroutine(LoadScene("GameStartScene"));
    }

    // Load the Main Game Scene (Level 1)
    public void LoadMainScene()
    {
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
        SceneManager.LoadScene("GameOptionsScene");
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}