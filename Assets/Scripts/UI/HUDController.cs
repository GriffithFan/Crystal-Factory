using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text coinsPerSecondText;
    [SerializeField] private TMP_Text clickPowerText;
    [SerializeField] private TMP_Text statusText;

    [Header("Buttons")]
    [SerializeField] private Button crystalButton;
    [SerializeField] private Button dailyRewardButton;
    [SerializeField] private Button rewardedAdButton;

    private int sessionCrystalTaps;

    private void OnEnable()
    {
        GameEvents.CoinsChanged += UpdateCoins;
        GameEvents.CoinsPerSecondChanged += UpdateCoinsPerSecond;
        GameEvents.ClickPowerChanged += UpdateClickPower;
        GameEvents.OfflineRewardCalculated += ShowOfflineReward;
        GameEvents.BoostChanged += ShowBoost;

        if (crystalButton != null)
        {
            crystalButton.onClick.AddListener(OnCrystalClicked);
        }

        if (dailyRewardButton != null)
        {
            dailyRewardButton.onClick.AddListener(OnDailyRewardClicked);
        }

        if (rewardedAdButton != null)
        {
            rewardedAdButton.onClick.AddListener(OnRewardedAdClicked);
        }
    }

    private void Start()
    {
        RefreshDailyRewardButton();
        SetStatus("Pedido activo: despierta el nucleo con 10 toques y compra Pulidor Manual.");
    }

    private void OnDisable()
    {
        GameEvents.CoinsChanged -= UpdateCoins;
        GameEvents.CoinsPerSecondChanged -= UpdateCoinsPerSecond;
        GameEvents.ClickPowerChanged -= UpdateClickPower;
        GameEvents.OfflineRewardCalculated -= ShowOfflineReward;
        GameEvents.BoostChanged -= ShowBoost;

        if (crystalButton != null)
        {
            crystalButton.onClick.RemoveListener(OnCrystalClicked);
        }

        if (dailyRewardButton != null)
        {
            dailyRewardButton.onClick.RemoveListener(OnDailyRewardClicked);
        }

        if (rewardedAdButton != null)
        {
            rewardedAdButton.onClick.RemoveListener(OnRewardedAdClicked);
        }
    }

    private void OnCrystalClicked()
    {
        if (GameManager.Instance == null)
        {
            SetStatus("El sistema aun esta iniciando. Espera un segundo.");
            return;
        }

        GameManager.Instance.ClickMainCrystal();
        sessionCrystalTaps++;

        if (sessionCrystalTaps < 10)
        {
            SetStatus("Energia inicial: " + sessionCrystalTaps + "/10. Sigue tocando para encender Aurora.");
        }
        else
        {
            SetStatus("Nucleo estable. Compra Pulidor Manual para aumentar el valor de cada envio.");
        }
    }

    private void OnDailyRewardClicked()
    {
        if (GameManager.Instance == null)
        {
            SetStatus("El sistema aun esta iniciando. Espera un segundo.");
            return;
        }

        double reward = GameManager.Instance.ClaimDailyReward();

        if (reward > 0)
        {
            SetStatus($"Recompensa diaria: +{NumberFormatter.Format(reward)} cristales");
        }
        else
        {
            SetStatus("La recompensa diaria todavia no esta lista.");
        }

        RefreshDailyRewardButton();
    }

    private void OnRewardedAdClicked()
    {
        if (GameManager.Instance == null)
        {
            SetStatus("El sistema aun esta iniciando. Espera un segundo.");
            return;
        }

        GameManager.Instance.Ads.ShowRewardedAd();
        SetStatus("Boost comercial recibido. Usalo para acelerar pedidos, no para reemplazar mejoras.");
    }

    private void UpdateCoins(double coins)
    {
        if (coinsText != null)
        {
            coinsText.text = NumberFormatter.Format(coins);
        }
    }

    private void UpdateCoinsPerSecond(double coinsPerSecond)
    {
        if (coinsPerSecondText != null)
        {
            coinsPerSecondText.text = NumberFormatter.Format(coinsPerSecond) + "/s";
        }
    }

    private void UpdateClickPower(double clickPower)
    {
        if (clickPowerText != null)
        {
            clickPowerText.text = "+" + NumberFormatter.Format(clickPower);
        }
    }

    private void ShowOfflineReward(double reward)
    {
        SetStatus($"Progreso offline: +{NumberFormatter.Format(reward)} cristales");
    }

    private void ShowBoost(double multiplier, float remainingSeconds)
    {
        if (multiplier <= 1 || remainingSeconds <= 0)
        {
            SetStatus("Boost finalizado.");
            return;
        }

        SetStatus($"Boost x{NumberFormatter.Format(multiplier)} activo por {Mathf.CeilToInt(remainingSeconds)}s");
    }

    private void RefreshDailyRewardButton()
    {
        if (dailyRewardButton != null && GameManager.Instance != null)
        {
            dailyRewardButton.interactable = GameManager.Instance.DailyRewards.CanClaim;
        }
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
}
