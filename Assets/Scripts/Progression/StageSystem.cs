using UnityEngine;

public class StageSystem : MonoBehaviour
{
    private CurrencySystem currencySystem;
    private StageDefinition[] stages;
    private StageDefinition currentStage;

    public StageDefinition CurrentStage => currentStage;
    public StageDefinition[] Stages => stages;

    public void Initialize(CurrencySystem gameCurrencySystem, StageDefinition[] loadedStages)
    {
        currencySystem = gameCurrencySystem;
        stages = loadedStages;
        EvaluateStage(true);
    }

    private void OnEnable()
    {
        GameEvents.CoinsEarned += OnCoinsEarned;
    }

    private void OnDisable()
    {
        GameEvents.CoinsEarned -= OnCoinsEarned;
    }

    private void OnCoinsEarned(double amount)
    {
        EvaluateStage(false);
    }

    private void EvaluateStage(bool forceRaise)
    {
        if (stages == null || stages.Length == 0 || currencySystem == null)
        {
            return;
        }

        StageDefinition unlockedStage = stages[0];

        foreach (StageDefinition stage in stages)
        {
            if (currencySystem.TotalCoinsEarned >= stage.unlockAtTotalCoins)
            {
                unlockedStage = stage;
            }
        }

        if (!forceRaise && currentStage != null && currentStage.id == unlockedStage.id)
        {
            return;
        }

        currentStage = unlockedStage;
        GameEvents.RaiseStageChanged(currentStage);
    }
}
