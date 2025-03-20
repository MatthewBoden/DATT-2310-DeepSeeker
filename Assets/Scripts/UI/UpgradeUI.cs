using Player;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private TMP_Text gemCountText;
    [SerializeField] private GameObject upgradeMenu;

    [SerializeField] private int baseGemCost = 5;
    [SerializeField] private float costMultiplier = 1.5f;
    [SerializeField] private int maxUpgradesPerStat = 5;

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

    [SerializeField] private TMP_Text strengthCostText;
    [SerializeField] private TMP_Text healthCostText;
    [SerializeField] private TMP_Text staminaCostText;
    [SerializeField] private TMP_Text flashlightCostText;
    [SerializeField] private TMP_Text fortuneCostText;

    private void Start()
    {
        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
        }
        UpdateGemCount();
        UpdateUpgradeUI();
    }

    private void Update()
    {
        if (upgradeMenu.activeSelf)
        {
            UpdateGemCount();
        }
    }

    private void UpdateGemCount()
    {
        if (gemCountText != null)
        {
            gemCountText.text = inventoryManager.GetGemCount().ToString();
        }
    }

    private int GetUpgradeCost(int upgradeLevel)
    {
        return Mathf.RoundToInt(baseGemCost * Mathf.Pow(costMultiplier, upgradeLevel));
    }

    private void UpdateUpgradeUI()
    {
        Dictionary<string, int> upgradeLevels = playerController.GetUpgradeLevels();

        strengthUpgrades = upgradeLevels["strength"];
        healthUpgrades = upgradeLevels["maxHealth"];
        staminaUpgrades = upgradeLevels["maxStamina"];
        flashlightUpgrades = upgradeLevels["flashlightStat"];
        fortuneUpgrades = upgradeLevels["fortune"];

        strengthText.text = $"{strengthUpgrades}/{maxUpgradesPerStat}";
        healthText.text = $"{healthUpgrades}/{maxUpgradesPerStat}";
        staminaText.text = $"{staminaUpgrades}/{maxUpgradesPerStat}";
        flashlightText.text = $"{flashlightUpgrades}/{maxUpgradesPerStat}";
        fortuneText.text = $"{fortuneUpgrades}/{maxUpgradesPerStat}";

        strengthCostText.text = strengthUpgrades >= maxUpgradesPerStat ? "MAXED" : $"Cost: {GetUpgradeCost(strengthUpgrades)}";
        healthCostText.text = healthUpgrades >= maxUpgradesPerStat ? "MAXED" : $"Cost: {GetUpgradeCost(healthUpgrades)}";
        staminaCostText.text = staminaUpgrades >= maxUpgradesPerStat ? "MAXED" : $"Cost: {GetUpgradeCost(staminaUpgrades)}";
        flashlightCostText.text = flashlightUpgrades >= maxUpgradesPerStat ? "MAXED" : $"Cost: {GetUpgradeCost(flashlightUpgrades)}";
        fortuneCostText.text = fortuneUpgrades >= maxUpgradesPerStat ? "MAXED" : $"Cost: {GetUpgradeCost(fortuneUpgrades)}";
    }

    public void UpgradeStrength()
    {
        if (strengthUpgrades < maxUpgradesPerStat && playerController.UpgradeStat("strength"))
        {
            strengthUpgrades++;
            UpdateGemCount();
            UpdateUpgradeUI();
        }
    }

    public void UpgradeHealth()
    {
        if (healthUpgrades < maxUpgradesPerStat && playerController.UpgradeStat("maxHealth"))
        {
            healthUpgrades++;
            UpdateGemCount();
            UpdateUpgradeUI();
        }
    }

    public void UpgradeStamina()
    {
        if (staminaUpgrades < maxUpgradesPerStat && playerController.UpgradeStat("maxStamina"))
        {
            staminaUpgrades++;
            UpdateGemCount();
            UpdateUpgradeUI();
        }
    }

    public void UpgradeFlashlight()
    {
        if (flashlightUpgrades < maxUpgradesPerStat && playerController.UpgradeStat("flashlightStat"))
        {
            flashlightUpgrades++;
            UpdateGemCount();
            UpdateUpgradeUI();
        }
    }

    public void UpgradeFortune()
    {
        if (fortuneUpgrades < maxUpgradesPerStat && playerController.UpgradeStat("fortune"))
        {
            fortuneUpgrades++;
            UpdateGemCount();
            UpdateUpgradeUI();
        }
    }
}
