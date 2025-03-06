using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private TMP_Text gemCountText; // Reference to the UI text for gems
    [SerializeField] private GameObject upgradeMenu;

    [SerializeField] private int gemCostPerUpgrade = 5;
    [SerializeField] private int maxUpgradesPerStat = 5; // Maximum upgrade limit per stat

    private int strengthUpgrades = 0;
    private int healthUpgrades = 0;
    private int staminaUpgrades = 0;
    private int flashlightUpgrades = 0;
    private int fortuneUpgrades = 0;

    [SerializeField] private TMP_Text strengthText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text staminaText;
    [SerializeField] private TMP_Text flashlightText;
    [SerializeField] private TMP_Text fortuneText;

    private void Start()
    {
        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
        }
        UpdateGemCount();
    }

    private void Update()
    {
        if (upgradeMenu.activeSelf) // Only update when menu is visible
        {
            UpdateGemCount();
        }
    }

    private void UpdateGemCount()
    {
        if (gemCountText != null)
        {
            gemCountText.text = "" + inventoryManager.GetGemCount();
        }
    }

    public void UpgradeStrength()
    {
        if (strengthUpgrades < maxUpgradesPerStat && playerController.UpgradeStat("strength", gemCostPerUpgrade))
        {
            strengthUpgrades++;
            strengthText.text = strengthUpgrades + "/" + maxUpgradesPerStat;
            UpdateGemCount();
        }
    }

    public void UpgradeHealth()
    {
        if (healthUpgrades < maxUpgradesPerStat && playerController.UpgradeStat("maxHealth", gemCostPerUpgrade))
        {
            healthUpgrades++;
            healthText.text = healthUpgrades + "/" + maxUpgradesPerStat;
            UpdateGemCount();
        }
    }

    public void UpgradeStamina()
    {
        if (staminaUpgrades < maxUpgradesPerStat && playerController.UpgradeStat("maxStamina", gemCostPerUpgrade))
        {
            staminaUpgrades++;
            staminaText.text = staminaUpgrades + "/" + maxUpgradesPerStat;
            UpdateGemCount();
        }
    }

    public void UpgradeFlashlight()
    {
        if (flashlightUpgrades < maxUpgradesPerStat && playerController.UpgradeStat("flashlightStat", gemCostPerUpgrade))
        {
            flashlightUpgrades++;
            flashlightText.text = flashlightUpgrades + "/" + maxUpgradesPerStat;
            UpdateGemCount();
        }
    }

    public void UpgradeFortune()
    {
        if (fortuneUpgrades < maxUpgradesPerStat && playerController.UpgradeStat("fortune", gemCostPerUpgrade))
        {
            fortuneUpgrades++;
            fortuneText.text = fortuneUpgrades + "/" + maxUpgradesPerStat;
            UpdateGemCount();
        }
    }

}
