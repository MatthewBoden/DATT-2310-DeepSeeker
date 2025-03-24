using Audio;
using Player;
using UI;
using UnityEngine;

namespace Ore
{
    public class Gem : MonoBehaviour
    {
        [SerializeField] private Item itemToLoot;  // Manually assign in Inspector
        [SerializeField] private AudioClip collectSound;
        
        private InventoryManager inventoryManager;
        private SingletonAudioManager _audioManager;

        private void Start()
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            _audioManager = SingletonAudioManager.Instance;

            if (inventoryManager == null)
                Debug.LogError("InventoryManager not found in the scene!");

            if (itemToLoot == null)
                Debug.LogError("ItemToLoot is not assigned! Please assign Ore1 in the Inspector.");
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;

            if (inventoryManager != null && itemToLoot != null)
            {
                bool result = inventoryManager.AddItem(itemToLoot);
                if (result)
                    Destroy(gameObject);
                else
                    Debug.LogWarning("Inventory is full or item could not be added.");
            }
            
            _audioManager.PlaySoundEffect(collectSound);
        }
    }
}
