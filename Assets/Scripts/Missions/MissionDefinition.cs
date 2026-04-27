using System;

public enum MissionType
{
    Clicks,
    UpgradesPurchased,
    RewardedAdsWatched,
    DailyRewardsClaimed,
    CoinsEarned
}

[Serializable]
public class MissionDefinition
{
    public string id;
    public string displayName;
    public string description;
    public MissionType type;
    public double target;
    public double rewardCoins;
}

[Serializable]
public class MissionCatalog
{
    public MissionDefinition[] missions;
}

[Serializable]
public class MissionSaveData
{
    public string missionId;
    public double progress;
    public bool completed;
    public bool claimed;
}
