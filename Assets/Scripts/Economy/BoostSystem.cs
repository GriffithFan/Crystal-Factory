using UnityEngine;

public class BoostSystem : MonoBehaviour
{
    [SerializeField] private double rewardedAdMultiplier = 2;
    [SerializeField] private float rewardedAdBoostSeconds = 180;

    private CurrencySystem currencySystem;
    private float remainingSeconds;

    public bool IsActive => remainingSeconds > 0;
    public float RemainingSeconds => remainingSeconds;

    public void Initialize(CurrencySystem gameCurrencySystem)
    {
        currencySystem = gameCurrencySystem;
    }

    private void OnEnable()
    {
        GameEvents.RewardedAdCompleted += OnRewardedAdCompleted;
    }

    private void OnDisable()
    {
        GameEvents.RewardedAdCompleted -= OnRewardedAdCompleted;
    }

    private void Update()
    {
        if (remainingSeconds <= 0)
        {
            return;
        }

        remainingSeconds -= Time.deltaTime;

        if (remainingSeconds <= 0)
        {
            remainingSeconds = 0;
            currencySystem.SetTemporaryMultiplier(1);
            GameEvents.RaiseBoostChanged(1, 0);
        }
    }

    private void OnRewardedAdCompleted(double reward)
    {
        ActivateBoost(rewardedAdMultiplier, rewardedAdBoostSeconds);
    }

    public void ActivateBoost(double multiplier, float seconds)
    {
        remainingSeconds = Mathf.Max(remainingSeconds, seconds);
        currencySystem.SetTemporaryMultiplier(multiplier);
        GameEvents.RaiseBoostChanged(multiplier, remainingSeconds);
    }
}
