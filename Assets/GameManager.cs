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

    private bool statsSaved = false; // Ensure we only load if stats exist

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerStats(PlayerController player)
    {
        playerHealth = player.GetHealth();
        playerMaxHealth = player.GetMaxHealth();
        playerStrength = player.GetStrength();
        playerStamina = player.GetStamina();
        playerFortune = player.GetFortune();
        playerFlashlightStat = player.GetFlashlightStat();
        gemCount = player.GetGemCount();
        upgradeLevels = new Dictionary<string, int>(player.GetUpgradeLevels());

        statsSaved = true;

        Debug.Log($"Saved Stats -> Health: {playerHealth}/{playerMaxHealth}, Gems: {gemCount}");
    }

    public void LoadPlayerStats(PlayerController player)
    {
        if (!statsSaved)
        {
            Debug.LogWarning("No saved stats found. Using default values.");
            return;
        }

        player.SetStats(
            playerHealth,
            playerMaxHealth,
            playerStrength,
            playerStamina,
            playerFortune,
            playerFlashlightStat,
            gemCount,
            upgradeLevels
        );

        Debug.Log($"Stats Loaded -> Health: {player.GetHealth()}/{player.GetMaxHealth()}, Gems: {player.GetGemCount()}");
    }
}
