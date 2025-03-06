using Player;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private int gemCostPerUpgrade = 5;

    public void UpgradeStrength()
    {
        playerController.UpgradeStat("strength", gemCostPerUpgrade);
    }

    public void UpgradeHealth()
    {
        playerController.UpgradeStat("maxHealth", gemCostPerUpgrade);
    }

    public void UpgradeStamina()
    {
        playerController.UpgradeStat("maxStamina", gemCostPerUpgrade);
    }

    public void UpgradeFlashlight()
    {
        playerController.UpgradeStat("flashlightStat", gemCostPerUpgrade);
    }

    public void UpgradeFortune()
    {
        playerController.UpgradeStat("fortune", gemCostPerUpgrade);
    }
}
