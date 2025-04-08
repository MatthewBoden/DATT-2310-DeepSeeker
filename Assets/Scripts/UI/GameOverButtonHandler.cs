using UnityEngine;

namespace UI
{
    public class GameOverButtonHandler : MonoBehaviour
    {
        private ScenesManager _scenesManager;

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

        public void OnClickMenu()
        {
            _scenesManager.LoadStartScene();
        }

        public void OnClickQuit()
        {
            _scenesManager.QuitGame();
        }
    }
}