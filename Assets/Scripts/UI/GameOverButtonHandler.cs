using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverButtonHandler : MonoBehaviour
    {
        [SerializeField] private AudioClip clickSound;
        
        private ScenesManager _scenesManager;
        private SingletonAudioManager _singletonAudioManager;

        private void Start()
        {
            _scenesManager = ScenesManager.Instance;
        }

        public void OnClickPlayAgainLevel1()
        {
            _scenesManager.LoadMainScene();
        }

        public void OnClickPlayAgainLevel2()
        {
            _scenesManager.LoadLevel2();
        }

        // This has the same functionality as `OnClickPlayAgainLevel2`, but kept separate to be tidy and accommodate future updates
        public void OnClickNextLevel()
        {
            _scenesManager.LoadLevel2();
        }

        public void OnClickMenu()
        {
            _scenesManager.LoadStartScene();
        }
        
        public void OnClickMenuNoTransition()
        {
            SceneManager.LoadScene("GameStartScene");
        }

        public void OnClickOptionsNoTransition()
        {
            SceneManager.LoadScene("GameOptionsScene");
        }

        public void OnClickQuit()
        {
            _scenesManager.QuitGame();
        }
    }
}