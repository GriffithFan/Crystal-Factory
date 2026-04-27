using System;
using System.Globalization;
using UnityEngine;

public class DailyRewardSystem : MonoBehaviour
{
    [SerializeField] private double baseDailyReward = 100;
    [SerializeField] private int maxStreakBonusDays = 7;

    private GameState state;
    private CurrencySystem currencySystem;
    private SaveManager saveManager;

    public bool CanClaim { get; private set; }
    public int CurrentStreak => state.dailyRewardStreak;

    public void Initialize(GameState gameState, CurrencySystem gameCurrencySystem, SaveManager gameSaveManager)
    {
        state = gameState;
        currencySystem = gameCurrencySystem;
        saveManager = gameSaveManager;
        RefreshClaimState();
    }

    public double Claim()
    {
        RefreshClaimState();

        if (!CanClaim)
        {
            return 0;
        }

        UpdateStreak();
        double reward = baseDailyReward * Math.Max(1, Math.Min(state.dailyRewardStreak, maxStreakBonusDays));
        currencySystem.AddCoins(reward, false);
        state.lastDailyRewardUtc = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture);
        saveManager.Save(state);
        RefreshClaimState();
        return reward;
    }

    public void RefreshClaimState()
    {
        if (string.IsNullOrWhiteSpace(state.lastDailyRewardUtc))
        {
            CanClaim = true;
            return;
        }

        if (!DateTime.TryParse(state.lastDailyRewardUtc, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime lastClaimUtc))
        {
            CanClaim = true;
            return;
        }

        CanClaim = DateTime.UtcNow.Date > lastClaimUtc.Date;
    }

    private void UpdateStreak()
    {
        if (string.IsNullOrWhiteSpace(state.lastDailyRewardUtc))
        {
            state.dailyRewardStreak = 1;
            return;
        }

        DateTime.TryParse(state.lastDailyRewardUtc, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime lastClaimUtc);
        double daysSinceLastClaim = (DateTime.UtcNow.Date - lastClaimUtc.Date).TotalDays;

        if (daysSinceLastClaim <= 1)
        {
            state.dailyRewardStreak += 1;
            return;
        }

        state.dailyRewardStreak = 1;
    }
}
