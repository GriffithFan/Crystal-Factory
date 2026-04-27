using System;

public static class GameEvents
{
    public static event Action<double> CoinsChanged;
    public static event Action<double> CoinsEarned;
    public static event Action<double> CoinsPerSecondChanged;
    public static event Action<double> ClickPowerChanged;
    public static event Action MainCrystalClicked;
    public static event Action<string, int> UpgradeLevelChanged;
    public static event Action<double> DailyRewardClaimed;
    public static event Action<double> RewardedAdCompleted;
    public static event Action<double, float> BoostChanged;
    public static event Action<double> OfflineRewardCalculated;
    public static event Action MissionsChanged;
    public static event Action<string> MissionCompleted;
    public static event Action<string, double> MissionClaimed;
    public static event Action<StageDefinition> StageChanged;

    public static void RaiseCoinsChanged(double coins)
    {
        CoinsChanged?.Invoke(coins);
    }

    public static void RaiseCoinsEarned(double amount)
    {
        CoinsEarned?.Invoke(amount);
    }

    public static void RaiseCoinsPerSecondChanged(double coinsPerSecond)
    {
        CoinsPerSecondChanged?.Invoke(coinsPerSecond);
    }

    public static void RaiseClickPowerChanged(double clickPower)
    {
        ClickPowerChanged?.Invoke(clickPower);
    }

    public static void RaiseMainCrystalClicked()
    {
        MainCrystalClicked?.Invoke();
    }

    public static void RaiseUpgradeLevelChanged(string upgradeId, int level)
    {
        UpgradeLevelChanged?.Invoke(upgradeId, level);
    }

    public static void RaiseDailyRewardClaimed(double reward)
    {
        DailyRewardClaimed?.Invoke(reward);
    }

    public static void RaiseRewardedAdCompleted(double reward)
    {
        RewardedAdCompleted?.Invoke(reward);
    }

    public static void RaiseBoostChanged(double multiplier, float remainingSeconds)
    {
        BoostChanged?.Invoke(multiplier, remainingSeconds);
    }

    public static void RaiseOfflineRewardCalculated(double reward)
    {
        OfflineRewardCalculated?.Invoke(reward);
    }

    public static void RaiseMissionsChanged()
    {
        MissionsChanged?.Invoke();
    }

    public static void RaiseMissionCompleted(string missionId)
    {
        MissionCompleted?.Invoke(missionId);
    }

    public static void RaiseMissionClaimed(string missionId, double reward)
    {
        MissionClaimed?.Invoke(missionId, reward);
    }

    public static void RaiseStageChanged(StageDefinition stage)
    {
        StageChanged?.Invoke(stage);
    }
}
