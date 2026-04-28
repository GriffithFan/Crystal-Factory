using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonView : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button buyButton;

    private UpgradeDefinition upgrade;

    public void Initialize(UpgradeDefinition upgradeDefinition)
    {
        upgrade = upgradeDefinition;

        if (buyButton != null)
        {
            buyButton.onClick.AddListener(Buy);
        }

        Refresh();
    }

    private void OnEnable()
    {
        GameEvents.CoinsChanged += OnCoinsChanged;
        GameEvents.UpgradeLevelChanged += OnUpgradeLevelChanged;
    }

    private void OnDisable()
    {
        GameEvents.CoinsChanged -= OnCoinsChanged;
        GameEvents.UpgradeLevelChanged -= OnUpgradeLevelChanged;

        if (buyButton != null)
        {
            buyButton.onClick.RemoveListener(Buy);
        }
    }

    private void Buy()
    {
        if (GameManager.Instance.Upgrades.Buy(upgrade))
        {
            Refresh();
        }
    }

    private void Refresh()
    {
        if (upgrade == null || GameManager.Instance == null)
        {
            return;
        }

        int level = GameManager.Instance.Upgrades.GetLevel(upgrade.id);
        double cost = GameManager.Instance.Upgrades.GetCost(upgrade);
        bool unlocked = GameManager.Instance.Upgrades.IsUnlocked(upgrade);
        bool canBuy = GameManager.Instance.Upgrades.CanBuy(upgrade);

        if (nameText != null)
        {
            nameText.text = upgrade.displayName;
        }

        if (descriptionText != null)
        {
            descriptionText.text = unlocked ? upgrade.description : "Bloqueado";
        }

        if (levelText != null)
        {
            levelText.text = "Nivel " + level;
        }

        if (costText != null)
        {
            costText.text = unlocked ? NumberFormatter.Format(cost) : "Requiere " + NumberFormatter.Format(upgrade.unlockAtTotalCoins);
        }

        if (iconImage != null)
        {
            Sprite icon = string.IsNullOrEmpty(upgrade.iconResource) ? null : Resources.Load<Sprite>(upgrade.iconResource);
            iconImage.sprite = icon;
            iconImage.enabled = icon != null;
            iconImage.preserveAspect = true;
        }

        if (buyButton != null)
        {
            buyButton.interactable = canBuy;
        }
    }

    private void OnCoinsChanged(double coins)
    {
        Refresh();
    }

    private void OnUpgradeLevelChanged(string upgradeId, int level)
    {
        if (upgrade != null && upgrade.id == upgradeId)
        {
            Refresh();
        }
    }
}
