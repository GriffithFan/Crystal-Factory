using UnityEngine;

public class IAPManager : MonoBehaviour
{
    private CurrencySystem currencySystem;

    public bool RemoveAdsPurchased { get; private set; }

    public void Initialize(CurrencySystem gameCurrencySystem)
    {
        currencySystem = gameCurrencySystem;
        RemoveAdsPurchased = PlayerPrefs.GetInt("remove_ads_purchased", 0) == 1;
    }

    public void BuySmallCoinPack()
    {
        currencySystem.AddCoins(1000, true);
        Debug.Log("Simulated small coin pack purchase. Replace with Google Play Billing.");
    }

    public void BuyMediumCoinPack()
    {
        currencySystem.AddCoins(6000, true);
        Debug.Log("Simulated medium coin pack purchase. Replace with Google Play Billing.");
    }

    public void BuyRemoveAds()
    {
        RemoveAdsPurchased = true;
        PlayerPrefs.SetInt("remove_ads_purchased", 1);
        PlayerPrefs.Save();
        Debug.Log("Simulated remove ads purchase. Replace with Google Play Billing.");
    }
}
