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

    public bool AddItem(Item item, int count = 1)
    {
        int remaining = count;

        // Stack into existing slots if possible
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.item.stackable)
            {
                int spaceLeft = maxStackedItems - itemInSlot.count;
                int amountToAdd = Mathf.Min(remaining, spaceLeft);
                itemInSlot.count += amountToAdd;
                itemInSlot.RefreshCount();

                remaining -= amountToAdd;
                if (remaining <= 0) return true;
            }
        }

        // Create new stacks in empty slots
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
            {
                int amountToAdd = Mathf.Min(remaining, maxStackedItems);
                SpawnNewItem(item, slot, amountToAdd);
                remaining -= amountToAdd;

                if (remaining <= 0) return true;
            }
        }

        return remaining == 0; // Returns false if inventory is full
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
    public void ClearInventory()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                Destroy(itemInSlot.gameObject);
            }
        }
    }

    private void SpawnNewItem(Item item, InventorySlot slot, int count)
    {
        GameObject newItemGo = Instantiate(InventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item, count);
        inventoryItem.RefreshCount();
    }
}
