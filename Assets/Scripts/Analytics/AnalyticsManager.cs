using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    public void TrackGameStarted()
    {
        Debug.Log("Analytics event: game_start");
    }

    public void TrackUpgradePurchased(string upgradeId, int level)
    {
        Debug.Log($"Analytics event: upgrade_purchased upgrade={upgradeId} level={level}");
    }

    public void TrackRewardedAdCompleted(double reward)
    {
        Debug.Log($"Analytics event: ad_reward_completed reward={reward}");
    }

    public void TrackDailyRewardClaimed(double reward)
    {
        Debug.Log($"Analytics event: daily_reward_claimed reward={reward}");
    }

    public void TrackOfflineRewardClaimed(double reward)
    {
        Debug.Log($"Analytics event: offline_reward_claimed reward={reward}");
    }
}
