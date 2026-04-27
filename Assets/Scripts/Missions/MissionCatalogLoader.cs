using UnityEngine;

public class MissionCatalogLoader : MonoBehaviour
{
    [SerializeField] private string resourcePath = "GameData/MissionCatalog";

    public MissionDefinition[] LoadMissions()
    {
        TextAsset catalogAsset = Resources.Load<TextAsset>(resourcePath);

        if (catalogAsset == null)
        {
            Debug.LogError($"Mission catalog not found at Resources/{resourcePath}.json");
            return new MissionDefinition[0];
        }

        MissionCatalog catalog = UnityEngine.JsonUtility.FromJson<MissionCatalog>(catalogAsset.text);
        return catalog?.missions ?? new MissionDefinition[0];
    }
}
