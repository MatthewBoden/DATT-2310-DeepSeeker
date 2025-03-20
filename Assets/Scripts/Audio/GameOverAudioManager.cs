using UnityEngine;

namespace Audio
{
    public class GameOverAudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource soundEffectSource;
        [SerializeField] private AudioClip gameOverSound;

        private void Start() => soundEffectSource?.PlayOneShot(gameOverSound);
    }
}