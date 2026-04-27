using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private UpgradeButtonView upgradeButtonPrefab;

    private void Start()
    {
        BuildShop();
    }

    public void BuildShop()
    {
        if (container == null || upgradeButtonPrefab == null || GameManager.Instance == null)
        {
            return;
        }

        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        foreach (UpgradeDefinition upgrade in GameManager.Instance.Upgrades.Upgrades)
        {
            UpgradeButtonView view = Instantiate(upgradeButtonPrefab, container);
            view.Initialize(upgrade);
        }
    }
}
