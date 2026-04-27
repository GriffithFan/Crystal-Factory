using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingTextSpawner : MonoBehaviour
{
    [SerializeField] private TMP_Text floatingTextPrefab;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private Vector2 randomOffset = new Vector2(90, 30);
    [SerializeField] private float riseDistance = 120;
    [SerializeField] private float lifetime = 0.65f;

    private void OnEnable()
    {
        GameEvents.MainCrystalClicked += SpawnClickValue;
        GameEvents.MissionClaimed += SpawnMissionReward;
    }

    private void OnDisable()
    {
        GameEvents.MainCrystalClicked -= SpawnClickValue;
        GameEvents.MissionClaimed -= SpawnMissionReward;
    }

    private void SpawnClickValue()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        Spawn("+" + NumberFormatter.Format(GameManager.Instance.Currency.ClickPower));
    }

    private void SpawnMissionReward(string missionId, double reward)
    {
        Spawn("Mision +" + NumberFormatter.Format(reward));
    }

    public void Spawn(string message)
    {
        if (floatingTextPrefab == null)
        {
            return;
        }

        Transform parent = spawnParent == null ? transform : spawnParent;
        TMP_Text text = Instantiate(floatingTextPrefab, parent);
        text.text = message;

        RectTransform rect = text.GetComponent<RectTransform>();
        Vector2 offset = new Vector2(Random.Range(-randomOffset.x, randomOffset.x), Random.Range(-randomOffset.y, randomOffset.y));
        rect.anchoredPosition += offset;
        StartCoroutine(Animate(text, rect));
    }

    private IEnumerator Animate(TMP_Text text, RectTransform rect)
    {
        float elapsed = 0;
        Vector2 start = rect.anchoredPosition;
        Vector2 end = start + new Vector2(0, riseDistance);
        Color startColor = text.color;

        while (elapsed < lifetime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / lifetime);
            rect.anchoredPosition = Vector2.Lerp(start, end, t);
            text.color = new Color(startColor.r, startColor.g, startColor.b, 1 - t);
            yield return null;
        }

        Destroy(text.gameObject);
    }
}
