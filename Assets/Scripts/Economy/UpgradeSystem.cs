using System;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    private GameState state;
    private CurrencySystem currencySystem;
    private UpgradeDefinition[] upgrades;

    public UpgradeDefinition[] Upgrades => upgrades;

    public void Initialize(GameState gameState, CurrencySystem gameCurrencySystem, UpgradeDefinition[] loadedUpgrades)
    {
        state = gameState;
        currencySystem = gameCurrencySystem;
        upgrades = loadedUpgrades;
    }

    public int GetLevel(string upgradeId)
    {
        return state.GetUpgradeLevel(upgradeId);
    }

    public double GetCost(UpgradeDefinition upgrade)
    {
        int level = GetLevel(upgrade.id);
        return upgrade.baseCost * Math.Pow(upgrade.costGrowth, level);
    }

    public bool IsUnlocked(UpgradeDefinition upgrade)
    {
        return currencySystem.TotalCoinsEarned >= upgrade.unlockAtTotalCoins;
    }

    public bool CanBuy(UpgradeDefinition upgrade)
    {
        return IsUnlocked(upgrade) && currencySystem.Coins >= GetCost(upgrade);
    }

    public bool Buy(UpgradeDefinition upgrade)
    {
        if (!CanBuy(upgrade))
        {
            return false;
        }

        double cost = GetCost(upgrade);

        if (!currencySystem.SpendCoins(cost))
        {
            return false;
        }

        int nextLevel = GetLevel(upgrade.id) + 1;
        state.SetUpgradeLevel(upgrade.id, nextLevel);
        ApplyUpgrade(upgrade);
        GameEvents.RaiseUpgradeLevelChanged(upgrade.id, nextLevel);
        return true;
    }

    private void ApplyUpgrade(UpgradeDefinition upgrade)
    {
        if (upgrade.clickPowerBonus > 0)
        {
            currencySystem.AddClickPower(upgrade.clickPowerBonus);
        }

        if (upgrade.coinsPerSecondBonus > 0)
        {
            currencySystem.AddCoinsPerSecond(upgrade.coinsPerSecondBonus);
        }

        if (upgrade.globalMultiplierBonus > 0)
        {
            currencySystem.AddGlobalMultiplier(upgrade.globalMultiplierBonus);
        }
    }
}
