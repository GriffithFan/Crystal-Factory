using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private GameObject gameManagerPrefab;

    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            return;
        }

        if (gameManagerPrefab != null)
        {
            Instantiate(gameManagerPrefab);
            return;
        }

        Debug.LogWarning("Assign a GameManager prefab to Bootstrapper or place GameManager in the scene.");
    }
}
