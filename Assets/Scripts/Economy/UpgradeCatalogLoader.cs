using UnityEngine;

public class UpgradeCatalogLoader : MonoBehaviour
{
    [SerializeField] private string resourcePath = "GameData/UpgradeCatalog";

    public UpgradeDefinition[] LoadUpgrades()
    {
        TextAsset catalogAsset = Resources.Load<TextAsset>(resourcePath);

        if (catalogAsset == null)
        {
            Debug.LogError($"Upgrade catalog not found at Resources/{resourcePath}.json");
            return new UpgradeDefinition[0];
        }

        UpgradeCatalog catalog = JsonUtility.FromJson<UpgradeCatalog>(catalogAsset.text);
        return catalog?.upgrades ?? new UpgradeDefinition[0];
    }
}
