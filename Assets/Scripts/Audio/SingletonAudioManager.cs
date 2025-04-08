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
        [SerializeField] private AudioClip clickSound;

        public bool InGame { get; set; }

        private bool _isBackgroundMusicPlaying;

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

        private void Update()
        {
            if (InGame && !_isBackgroundMusicPlaying)
            {
                backgroundMusicSource.Play();
                _isBackgroundMusicPlaying = true;
            }
            else if (!InGame && _isBackgroundMusicPlaying)
            {
                backgroundMusicSource.Stop();
                _isBackgroundMusicPlaying = false;
            }
        }

        private void Start()
        {
            Debug.Log("Starting audio manager");
            backgroundMusicSource.clip = backgroundMusic;
            backgroundMusicSource.loop = true;
            // backgroundMusicSource.Play();

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

        public void PlayClickSound()
        {
            soundEffectSource?.PlayOneShot(clickSound);
        }
    }
}