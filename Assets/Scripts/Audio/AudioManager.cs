using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        // private GameManager _gameManager;
        [SerializeField] private AudioSource backgroundMusicSource;
        [SerializeField] private AudioSource soundEffectSource;
        [SerializeField] private AudioClip backgroundMusic;
        // [SerializeField] private AudioClip soundEffectGameOver;

        private void Awake()
        {
            if (Instance == null)
            {
                Debug.Log("Initializing singleton");
                Instance = this;
            }
        }

        private void Start()
        {
            Debug.Log("Starting audio manager");
            backgroundMusicSource.clip = backgroundMusic;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        
            // TODO: Add appropriate logic to play game over SFX
            // _gameManager = GameManager.Instance;
            // _gameManager.AddListenerOnStart(() => backgroundMusicSource.Play());
            // _gameManager.AddListenerOnGameOver(() =>
            // {
            //     backgroundMusicSource.Stop();
            //     PlaySoundEffect(soundEffectGameOver);
            // });
        }

        public void PlaySoundEffect(AudioClip clip)
        {
            Debug.Log("Calling SFX");
            soundEffectSource?.PlayOneShot(clip);
        }
    }
}