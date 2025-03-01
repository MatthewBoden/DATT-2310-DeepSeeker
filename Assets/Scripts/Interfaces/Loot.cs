using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Item itemToLoot; // Assigned in Inspector

    private void Start()
    {
        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        if (itemToLoot != null && inventoryManager != null)
        {
            bool result = inventoryManager.AddItem(itemToLoot);
            if (result)
            {
                Debug.Log($"{itemToLoot.name} added to inventory!");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory full! Could not add item.");
            }
        }
        else
        {
            Debug.LogError("Missing InventoryManager or ItemToLoot reference!");
        }
    }
}
