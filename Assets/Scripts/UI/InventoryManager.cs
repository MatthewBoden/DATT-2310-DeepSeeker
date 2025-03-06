using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public int maxStackedItems = 64;
    public InventorySlot[] inventorySlots;
    public GameObject InventoryItemPrefab;

    private void Awake()
    {
        instance = this;
    }

    public bool AddItem(Item item)
    {
        // If any slot has the same item with count lower than max, they will be stacked
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItems && itemInSlot.item.stackable==true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }

        }

        // Find empty Slot
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null) {
                SpawnNewItem(item, slot);
                return true;
            }
            
        }

        return false; // If inventory is full, will not add to inventory
    }

    public bool RemoveGems(int amount)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item.type == Item.ItemType.ore)
            {
                if (itemInSlot.count >= amount)
                {
                    itemInSlot.count -= amount;
                    if (itemInSlot.count == 0)
                    {
                        Destroy(itemInSlot.gameObject); // Remove from inventory if empty
                    }
                    else
                    {
                        itemInSlot.RefreshCount();
                    }
                    return true; // Gems successfully removed
                }
            }
        }

        return false; // Not enough gems
    }

    public int GetGemCount()
    {
        int gemCount = 0;
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item.type == Item.ItemType.ore)
            {
                gemCount += itemInSlot.count;
            }
        }
        return gemCount;
    }



    void SpawnNewItem(Item item, InventorySlot slot) { 
        GameObject newItemGo = Instantiate(InventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }
}
