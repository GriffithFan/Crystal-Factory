using UnityEngine;

public class MissionPanelController : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private MissionRowView missionRowPrefab;

    private void OnEnable()
    {
        GameEvents.MissionsChanged += Refresh;
        GameEvents.MissionClaimed += OnMissionClaimed;
    }

    private void Start()
    {
        BuildMissionList();
    }

    private void OnDisable()
    {
        GameEvents.MissionsChanged -= Refresh;
        GameEvents.MissionClaimed -= OnMissionClaimed;
    }

    public void BuildMissionList()
    {
        if (container == null || missionRowPrefab == null || GameManager.Instance == null)
        {
            return;
        }

        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        foreach (MissionDefinition mission in GameManager.Instance.Missions.Missions)
        {
            MissionRowView row = Instantiate(missionRowPrefab, container);
            row.Initialize(mission);
        }
    }

    public void Refresh()
    {
        foreach (Transform child in container)
        {
            MissionRowView row = child.GetComponent<MissionRowView>();

            if (row != null)
            {
                row.Refresh();
            }
        }
    }

    private void OnMissionClaimed(string missionId, double reward)
    {
        Refresh();
    }
}
