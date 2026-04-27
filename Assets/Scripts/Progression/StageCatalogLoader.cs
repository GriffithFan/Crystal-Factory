using UnityEngine;

public class StageCatalogLoader : MonoBehaviour
{
    [SerializeField] private string resourcePath = "GameData/StageCatalog";

    public StageDefinition[] LoadStages()
    {
        TextAsset catalogAsset = Resources.Load<TextAsset>(resourcePath);

        if (catalogAsset == null)
        {
            Debug.LogError($"Stage catalog not found at Resources/{resourcePath}.json");
            return new StageDefinition[0];
        }

        StageCatalog catalog = JsonUtility.FromJson<StageCatalog>(catalogAsset.text);
        return catalog?.stages ?? new StageDefinition[0];
    }
}
