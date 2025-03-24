using UnityEngine;

namespace Audio
{
    public class MenuAudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip clip;

        private void Start() => source?.PlayOneShot(clip);
    }
}