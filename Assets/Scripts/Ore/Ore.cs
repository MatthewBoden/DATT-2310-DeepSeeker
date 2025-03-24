using Audio;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ore
{
    public class Ore : MonoBehaviour, IDamageable
    {
        [SerializeField] private float durability;
        [SerializeField] private GameObject mineral;
        [SerializeField] private int baseMineralCount;
        [SerializeField] private float mineralSpawnRadius;
        [SerializeField] private float mineralSpawnForce;
        [SerializeField] private AudioClip hitOreSound;
        [SerializeField] private AudioClip destroyOreSound;
        
        private SingletonAudioManager _singletonAudioManager;

        private void Start()
        {
            _singletonAudioManager = SingletonAudioManager.Instance;
        }

        public void Damage(float impact, float fortuneMultiplier)
        {
            if ((durability -= impact) > 0)
            {
                _singletonAudioManager.PlaySoundEffect(hitOreSound, 0.25f);
                return;
            }

            // Determine mineral count based on fortune
            var finalMineralCount = Mathf.CeilToInt(baseMineralCount * fortuneMultiplier);

            for (var i = 0; i < finalMineralCount; i++)
            {
                var spawnPosition = transform.position + (Vector3)(Random.insideUnitCircle * mineralSpawnRadius);
                var randomDirection = Random.insideUnitCircle.normalized;
                var instance = Instantiate(mineral, spawnPosition, Quaternion.identity);
                instance.GetComponent<Rigidbody2D>().AddForce(randomDirection * mineralSpawnForce, ForceMode2D.Impulse);
            }

            _singletonAudioManager.PlaySoundEffect(destroyOreSound, 0.25f);
            Destroy(gameObject);
        }
    }
}