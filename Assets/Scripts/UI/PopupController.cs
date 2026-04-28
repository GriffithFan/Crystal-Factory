using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button closeButton;

    private bool skippedInitialStage;

    private void OnEnable()
    {
        GameEvents.MissionCompleted += OnMissionCompleted;
        GameEvents.MissionClaimed += OnMissionClaimed;
        GameEvents.DailyRewardClaimed += OnDailyRewardClaimed;
        GameEvents.RewardedAdCompleted += OnRewardedAdCompleted;
        GameEvents.StageChanged += OnStageChanged;

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Hide);
        }
    }

    private void Start()
    {
        Hide();
    }

    private void OnDisable()
    {
        GameEvents.MissionCompleted -= OnMissionCompleted;
        GameEvents.MissionClaimed -= OnMissionClaimed;
        GameEvents.DailyRewardClaimed -= OnDailyRewardClaimed;
        GameEvents.RewardedAdCompleted -= OnRewardedAdCompleted;
        GameEvents.StageChanged -= OnStageChanged;

        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(Hide);
        }
    }

    public void Show(string title, string message)
    {
        if (titleText != null)
        {
            titleText.text = title;
        }

        if (messageText != null)
        {
            messageText.text = message;
        }

        if (root != null)
        {
            root.SetActive(true);
        }
    }

    public void Hide()
    {
        if (root != null)
        {
            root.SetActive(false);
        }
    }

    private void OnMissionCompleted(string missionId)
    {
        Show("Pedido listo", "Hay una mision completada. Reclama la recompensa para acelerar la restauracion de Aurora.");
    }

    private void OnMissionClaimed(string missionId, double reward)
    {
        Show("Envio cobrado", "+" + NumberFormatter.Format(reward) + " cristales recibidos por cumplir un pedido.");
    }

    private void OnDailyRewardClaimed(double reward)
    {
        Show("Suministro diario", "+" + NumberFormatter.Format(reward) + " cristales para mantener la fabrica activa.");
    }

    private void OnRewardedAdCompleted(double reward)
    {
        Show("Boost comercial", "+" + NumberFormatter.Format(reward) + " cristales. Los anuncios son aceleradores opcionales.");
    }

    private void OnStageChanged(StageDefinition stage)
    {
        if (!skippedInitialStage)
        {
            skippedInitialStage = true;
            return;
        }

        Show("Sector restaurado", stage.displayName + " vuelve a estar en linea.");
    }
}
