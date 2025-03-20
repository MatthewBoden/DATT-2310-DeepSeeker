using Player;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private float playerHealth;
    private float playerMaxHealth;
    private float playerStrength;
    private float playerStamina;
    private float playerFortune;
    private float playerFlashlightStat;
    private int gemCount;
    private Dictionary<string, int> upgradeLevels = new Dictionary<string, int>();

    private List<Item> inventoryItems = new List<Item>();
    private List<int> inventoryCounts = new List<int>();

    private bool statsSaved = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerData(PlayerController player, InventoryManager inventory)
    {
        playerHealth = player.GetHealth();
        playerMaxHealth = player.GetMaxHealth();
        playerStrength = player.GetStrength();
        playerStamina = player.GetStamina();
        playerFortune = player.GetFortune();
        playerFlashlightStat = player.GetFlashlightStat();
        upgradeLevels = new Dictionary<string, int>(player.GetUpgradeLevels());

        inventoryItems.Clear();
        inventoryCounts.Clear();

        foreach (var slot in inventory.inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                inventoryItems.Add(itemInSlot.item);
                inventoryCounts.Add(itemInSlot.count);
            }
        }

        statsSaved = true;

        Debug.Log($"Saved Inventory -> {inventoryItems.Count} stacks.");
    }

    public void LoadPlayerData(PlayerController player, InventoryManager inventory)
    {
        if (!statsSaved)
        {
            Debug.LogWarning("No saved data found.");
            return;
        }

        player.SetStats(
            playerHealth,
            playerMaxHealth,
            playerStrength,
            playerStamina,
            playerFortune,
            playerFlashlightStat,
            upgradeLevels
        );

        inventory.ClearInventory();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventory.AddItem(inventoryItems[i], inventoryCounts[i]);
        }

        Debug.Log($"Inventory Loaded -> {inventoryItems.Count} stacks.");
    }
}
