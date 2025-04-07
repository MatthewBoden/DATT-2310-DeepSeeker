using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private Button playAgainButton;
        [SerializeField] private UnityEvent onPlayAgainClicked;

        private ScenesManager _scenesManager;

        private void Start()
        {
            _scenesManager = ScenesManager.Instance;

            playAgainButton.onClick.AddListener(() => _scenesManager.LoadMainScene());
        }
    }
}