using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverButtonHandler : MonoBehaviour
    {
        private ScenesManager _scenesManager;
        private SingletonAudioManager _singletonAudioManager;

        private void Start()
        {
            _scenesManager = ScenesManager.Instance;
            _singletonAudioManager = SingletonAudioManager.Instance;
        }

        public void OnClickPlayAgainLevel1()
        {
            _singletonAudioManager.PlayClickSound();
            _scenesManager.LoadMainScene();
        }

        public void OnClickPlayAgainLevel2()
        {
            _singletonAudioManager.PlayClickSound();
            _scenesManager.LoadLevel2();
        }

        // This has the same functionality as `OnClickPlayAgainLevel2`, but kept separate to be tidy and accommodate future updates
        public void OnClickNextLevel()
        {
            _singletonAudioManager.PlayClickSound();
            _scenesManager.LoadLevel2();
        }

        public void OnClickMenu()
        {
            _singletonAudioManager.PlayClickSound();
            _scenesManager.LoadStartScene();
        }
        
        public void OnClickMenuNoTransition()
        {
            _singletonAudioManager.PlayClickSound();
            SceneManager.LoadScene("GameStartScene");
        }

        public void OnClickOptionsNoTransition()
        {
            _singletonAudioManager.PlayClickSound();
            SceneManager.LoadScene("GameOptionsScene");
        }

        public void OnClickQuit()
        {
            _singletonAudioManager.PlayClickSound();
            _scenesManager.QuitGame();
        }
    }
}