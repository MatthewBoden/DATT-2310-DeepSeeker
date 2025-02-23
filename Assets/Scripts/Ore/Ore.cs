using Interfaces;
using UnityEngine;

namespace Ore
{
    public class Ore : MonoBehaviour, IDamageable
    {
        [SerializeField] private float durability;
        [SerializeField] private GameObject mineral;
        [SerializeField] private int mineralCount;
        [SerializeField] private float mineralSpawnRadius;
        [SerializeField] private float mineralSpawnForce;

        public void Damage(float impact)
        {
            if ((durability -= impact) > 0) return;

            for (var i = 0; i < mineralCount; i++)
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