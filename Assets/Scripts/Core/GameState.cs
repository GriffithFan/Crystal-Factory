using System;
using System.Collections.Generic;

[Serializable]
public class GameState
{
    public double coins;
    public double totalCoinsEarned;
    public double clickPower = 1;
    public double coinsPerSecond;
    public double globalMultiplier = 1;
    public int dailyRewardStreak;
    public string lastDailyRewardUtc;
    public string lastSavedUtc;
    public List<UpgradeSaveData> upgrades = new List<UpgradeSaveData>();
    public List<MissionSaveData> missions = new List<MissionSaveData>();

    public int GetUpgradeLevel(string upgradeId)
    {
        UpgradeSaveData upgrade = upgrades.Find(item => item.upgradeId == upgradeId);
        return upgrade == null ? 0 : upgrade.level;
    }

    public void SetUpgradeLevel(string upgradeId, int level)
    {
        UpgradeSaveData upgrade = upgrades.Find(item => item.upgradeId == upgradeId);

        if (upgrade == null)
        {
            upgrades.Add(new UpgradeSaveData
            {
                upgradeId = upgradeId,
                level = level
            });
            return;
        }

        upgrade.level = level;
    }
}

[Serializable]
public class UpgradeSaveData
{
    public string upgradeId;
    public int level;
}
