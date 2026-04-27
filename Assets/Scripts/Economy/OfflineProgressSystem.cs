using System;
using System.Globalization;
using UnityEngine;

public class OfflineProgressSystem : MonoBehaviour
{
    [SerializeField] private double maxOfflineHours = 8;

    private GameState state;
    private CurrencySystem currencySystem;

    public void Initialize(GameState gameState, CurrencySystem gameCurrencySystem)
    {
        state = gameState;
        currencySystem = gameCurrencySystem;
        GrantOfflineProgress();
    }

    private void GrantOfflineProgress()
    {
        if (string.IsNullOrWhiteSpace(state.lastSavedUtc))
        {
            return;
        }

        if (!DateTime.TryParse(state.lastSavedUtc, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime lastSavedUtc))
        {
            return;
        }

        TimeSpan elapsed = DateTime.UtcNow - lastSavedUtc;
        double cappedSeconds = Math.Min(elapsed.TotalSeconds, maxOfflineHours * 3600);
        double reward = Math.Max(0, cappedSeconds * currencySystem.CoinsPerSecond);

        if (reward <= 0)
        {
            return;
        }

        currencySystem.AddCoins(reward, true);
        GameEvents.RaiseOfflineRewardCalculated(reward);
    }
}
