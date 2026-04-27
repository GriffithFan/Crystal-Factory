using System;

[Serializable]
public class UpgradeDefinition
{
    public string id;
    public string displayName;
    public string description;
    public string type;
    public double baseCost;
    public double costGrowth;
    public double clickPowerBonus;
    public double coinsPerSecondBonus;
    public double globalMultiplierBonus;
    public int unlockAtTotalCoins;
}

[Serializable]
public class UpgradeCatalog
{
    public UpgradeDefinition[] upgrades;
}
