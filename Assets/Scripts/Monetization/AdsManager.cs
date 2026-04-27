using System;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    [SerializeField] private bool simulateAdsInEditor = true;
    [SerializeField] private double rewardedAdCoins = 250;

    private CurrencySystem currencySystem;

    public event Action<double> RewardedAdCompleted;

    public void Initialize(CurrencySystem gameCurrencySystem)
    {
        currencySystem = gameCurrencySystem;
    }

    public void ShowRewardedAd()
    {
        if (simulateAdsInEditor)
        {
            CompleteRewardedAd();
            return;
        }

        Debug.Log("Connect AdMob or Unity LevelPlay rewarded ad here.");
    }

    public void CompleteRewardedAd()
    {
        currencySystem.AddCoins(rewardedAdCoins, true);
        GameEvents.RaiseRewardedAdCompleted(rewardedAdCoins);
        RewardedAdCompleted?.Invoke(rewardedAdCoins);
    }
}
