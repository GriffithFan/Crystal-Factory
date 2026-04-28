using TMPro;
using UnityEngine;

public class ProgressIncentiveController : MonoBehaviour
{
    [SerializeField] private TMP_Text headlineText;
    [SerializeField] private TMP_Text progressText;

    private void OnEnable()
    {
        GameEvents.CoinsChanged += OnCoinsChanged;
        GameEvents.StageChanged += OnStageChanged;
        Refresh();
    }

    private void OnDisable()
    {
        GameEvents.CoinsChanged -= OnCoinsChanged;
        GameEvents.StageChanged -= OnStageChanged;
    }

    private void Start()
    {
        Refresh();
    }

    private void OnCoinsChanged(double coins)
    {
        Refresh();
    }

    private void OnStageChanged(StageDefinition stage)
    {
        Refresh();
    }

    private void Refresh()
    {
        if (GameManager.Instance == null || GameManager.Instance.Stages == null || GameManager.Instance.Currency == null)
        {
            SetTexts("Meta: despertar Aurora", "Produce energia para desbloquear el siguiente sector.");
            return;
        }

        StageDefinition nextStage = GetNextStage();
        double totalCoins = GameManager.Instance.Currency.TotalCoinsEarned;

        if (nextStage == null)
        {
            SetTexts("Aurora esta en expansion", "Todos los sectores iniciales estan activos. Sigue mejorando para dominar la red galactica.");
            return;
        }

        double remaining = nextStage.unlockAtTotalCoins - totalCoins;

        if (remaining < 0)
        {
            remaining = 0;
        }
        string headline = "Proxima recompensa: " + nextStage.displayName;
        string progress = "Faltan " + NumberFormatter.Format(remaining) + " cristales producidos para abrir un nuevo sector.";
        SetTexts(headline, progress);
    }

    private StageDefinition GetNextStage()
    {
        StageDefinition[] stages = GameManager.Instance.Stages.Stages;
        double totalCoins = GameManager.Instance.Currency.TotalCoinsEarned;

        if (stages == null)
        {
            return null;
        }

        foreach (StageDefinition stage in stages)
        {
            if (stage.unlockAtTotalCoins > totalCoins)
            {
                return stage;
            }
        }

        return null;
    }

    private void SetTexts(string headline, string progress)
    {
        if (headlineText != null)
        {
            headlineText.text = headline;
        }

        if (progressText != null)
        {
            progressText.text = progress;
        }
    }
}
