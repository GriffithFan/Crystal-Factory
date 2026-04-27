using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionRowView : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private Button claimButton;

    private MissionDefinition mission;

    public void Initialize(MissionDefinition missionDefinition)
    {
        mission = missionDefinition;

        if (claimButton != null)
        {
            claimButton.onClick.AddListener(Claim);
        }

        Refresh();
    }

    private void OnDisable()
    {
        if (claimButton != null)
        {
            claimButton.onClick.RemoveListener(Claim);
        }
    }

    public void Refresh()
    {
        if (mission == null || GameManager.Instance == null)
        {
            return;
        }

        MissionSaveData state = GameManager.Instance.Missions.GetMissionState(mission.id);
        double progress = state == null ? 0 : state.progress;
        bool completed = state != null && state.completed;
        bool claimed = state != null && state.claimed;

        if (titleText != null)
        {
            titleText.text = mission.displayName;
        }

        if (descriptionText != null)
        {
            descriptionText.text = mission.description;
        }

        if (progressText != null)
        {
            progressText.text = NumberFormatter.Format(progress) + " / " + NumberFormatter.Format(mission.target);
        }

        if (rewardText != null)
        {
            rewardText.text = "+" + NumberFormatter.Format(mission.rewardCoins);
        }

        if (claimButton != null)
        {
            claimButton.interactable = completed && !claimed;
        }
    }

    private void Claim()
    {
        if (mission == null || GameManager.Instance == null)
        {
            return;
        }

        GameManager.Instance.Missions.Claim(mission);
        Refresh();
    }
}
