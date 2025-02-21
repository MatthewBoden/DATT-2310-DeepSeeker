using UnityEngine;

namespace Ore
{
    public class Gem : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            Destroy(gameObject);
        }
    }
}