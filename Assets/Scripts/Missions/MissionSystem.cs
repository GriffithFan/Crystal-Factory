using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionSystem : MonoBehaviour
{
    private GameState state;
    private CurrencySystem currencySystem;
    private SaveManager saveManager;
    private MissionDefinition[] missions;
    private bool hasUnsavedProgress;
    private float saveTimer;

    public MissionDefinition[] Missions => missions;

    public void Initialize(GameState gameState, CurrencySystem gameCurrencySystem, SaveManager gameSaveManager, MissionDefinition[] loadedMissions)
    {
        state = gameState;
        currencySystem = gameCurrencySystem;
        saveManager = gameSaveManager;
        missions = loadedMissions;
        EnsureMissionState();
    }

    private void OnEnable()
    {
        GameEvents.MainCrystalClicked += OnMainCrystalClicked;
        GameEvents.UpgradeLevelChanged += OnUpgradePurchased;
        GameEvents.RewardedAdCompleted += OnRewardedAdCompleted;
        GameEvents.DailyRewardClaimed += OnDailyRewardClaimed;
        GameEvents.CoinsEarned += OnCoinsEarned;
    }

    private void OnDisable()
    {
        GameEvents.MainCrystalClicked -= OnMainCrystalClicked;
        GameEvents.UpgradeLevelChanged -= OnUpgradePurchased;
        GameEvents.RewardedAdCompleted -= OnRewardedAdCompleted;
        GameEvents.DailyRewardClaimed -= OnDailyRewardClaimed;
        GameEvents.CoinsEarned -= OnCoinsEarned;

        if (hasUnsavedProgress && saveManager != null)
        {
            saveManager.Save(state);
        }
    }

    private void Update()
    {
        if (!hasUnsavedProgress)
        {
            return;
        }

        saveTimer += Time.deltaTime;

        if (saveTimer < 2)
        {
            return;
        }

        saveTimer = 0;
        hasUnsavedProgress = false;
        saveManager.Save(state);
    }

    public MissionSaveData GetMissionState(string missionId)
    {
        return state.missions.Find(mission => mission.missionId == missionId);
    }

    public bool Claim(MissionDefinition mission)
    {
        MissionSaveData missionState = GetMissionState(mission.id);

        if (missionState == null || !missionState.completed || missionState.claimed)
        {
            return false;
        }

        missionState.claimed = true;
        currencySystem.AddCoins(mission.rewardCoins, false);
        saveManager.Save(state);
        GameEvents.RaiseMissionClaimed(mission.id, mission.rewardCoins);
        return true;
    }

    private void EnsureMissionState()
    {
        if (state.missions == null)
        {
            state.missions = new List<MissionSaveData>();
        }

        foreach (MissionDefinition mission in missions)
        {
            if (GetMissionState(mission.id) != null)
            {
                continue;
            }

            state.missions.Add(new MissionSaveData
            {
                missionId = mission.id,
                progress = 0,
                completed = false,
                claimed = false
            });
        }
    }

    private void AddProgress(MissionType missionType, double amount)
    {
        bool changed = false;

        foreach (MissionDefinition mission in missions)
        {
            if (mission.type != missionType)
            {
                continue;
            }

            MissionSaveData missionState = GetMissionState(mission.id);

            if (missionState == null || missionState.claimed)
            {
                continue;
            }

            missionState.progress = Math.Min(mission.target, missionState.progress + amount);

            if (!missionState.completed && missionState.progress >= mission.target)
            {
                missionState.completed = true;
                GameEvents.RaiseMissionCompleted(mission.id);
            }

            changed = true;
        }

        if (changed)
        {
            hasUnsavedProgress = true;
            GameEvents.RaiseMissionsChanged();
        }
    }

    private void OnMainCrystalClicked()
    {
        AddProgress(MissionType.Clicks, 1);
    }

    private void OnUpgradePurchased(string upgradeId, int level)
    {
        AddProgress(MissionType.UpgradesPurchased, 1);
    }

    private void OnRewardedAdCompleted(double reward)
    {
        AddProgress(MissionType.RewardedAdsWatched, 1);
    }

    private void OnDailyRewardClaimed(double reward)
    {
        AddProgress(MissionType.DailyRewardsClaimed, 1);
    }

    private void OnCoinsEarned(double amount)
    {
        AddProgress(MissionType.CoinsEarned, amount);
    }
}
