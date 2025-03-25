using UnityEngine;

namespace Audio
{
    public class SingletonAudioManager : MonoBehaviour
    {
        public static SingletonAudioManager Instance;
        // private GameManager _gameManager;
        [SerializeField] private AudioSource backgroundMusicSource;
        [SerializeField] private AudioSource soundEffectSource;
        [SerializeField] private AudioClip backgroundMusic;
        [SerializeField] private AudioClip soundEffectGameOver;

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

        public void PlaySoundEffect(AudioClip clip, float volume = 1)
        {
            soundEffectSource?.PlayOneShot(clip, volume);
        }

        public void PlayGameOverSound()
        {
            PlaySoundEffect(soundEffectGameOver);
        }
    }
}