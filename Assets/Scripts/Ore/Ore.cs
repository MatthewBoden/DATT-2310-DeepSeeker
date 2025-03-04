using Interfaces;
using UnityEngine;

namespace Ore
{
    public class Ore : MonoBehaviour, IDamageable
    {
        [SerializeField] private float durability;
        [SerializeField] private GameObject mineral;
        [SerializeField] private int baseMineralCount;
        [SerializeField] private float mineralSpawnRadius;
        [SerializeField] private float mineralSpawnForce;

        public void Damage(float impact, float fortuneMultiplier)
        {
            if ((durability -= impact) > 0) return;

            // Determine mineral count based on fortune
            int finalMineralCount = Mathf.CeilToInt(baseMineralCount * fortuneMultiplier);

            for (var i = 0; i < finalMineralCount; i++)
            {
                var spawnPosition = transform.position + (Vector3)(Random.insideUnitCircle * mineralSpawnRadius);
                var randomDirection = Random.insideUnitCircle.normalized;
                var instance = Instantiate(mineral, spawnPosition, Quaternion.identity);
                instance.GetComponent<Rigidbody2D>().AddForce(randomDirection * mineralSpawnForce, ForceMode2D.Impulse);
            }

            Destroy(gameObject);
        }
    }
}