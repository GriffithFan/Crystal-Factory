using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Systems")]
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private CurrencySystem currencySystem;
    [SerializeField] private UpgradeCatalogLoader upgradeCatalogLoader;
    [SerializeField] private UpgradeSystem upgradeSystem;
    [SerializeField] private OfflineProgressSystem offlineProgressSystem;
    [SerializeField] private DailyRewardSystem dailyRewardSystem;
    [SerializeField] private BoostSystem boostSystem;
    [SerializeField] private StageCatalogLoader stageCatalogLoader;
    [SerializeField] private StageSystem stageSystem;
    [SerializeField] private MissionCatalogLoader missionCatalogLoader;
    [SerializeField] private MissionSystem missionSystem;
    [SerializeField] private AdsManager adsManager;
    [SerializeField] private IAPManager iapManager;
    [SerializeField] private AnalyticsManager analyticsManager;

    private GameState state;

    public CurrencySystem Currency => currencySystem;
    public UpgradeSystem Upgrades => upgradeSystem;
    public DailyRewardSystem DailyRewards => dailyRewardSystem;
    public BoostSystem Boosts => boostSystem;
    public StageSystem Stages => stageSystem;
    public MissionSystem Missions => missionSystem;
    public AdsManager Ads => adsManager;
    public IAPManager IAP => iapManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        AutoWireSystems();
        SubscribeToEvents();
        InitializeSystems();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void Update()
    {
        currencySystem.Tick(Time.deltaTime);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            saveManager.Save(state);
        }
    }

    private void OnApplicationQuit()
    {
        saveManager.Save(state);
    }

    public void ClickMainCrystal()
    {
        currencySystem.AddClickCoins();
        GameEvents.RaiseMainCrystalClicked();
    }

    public double ClaimDailyReward()
    {
        double reward = dailyRewardSystem.Claim();

        if (reward > 0)
        {
            GameEvents.RaiseDailyRewardClaimed(reward);
            analyticsManager.TrackDailyRewardClaimed(reward);
        }

        return reward;
    }

    private void InitializeSystems()
    {
        state = saveManager.Load();
        UpgradeDefinition[] upgrades = upgradeCatalogLoader.LoadUpgrades();
        StageDefinition[] stages = stageCatalogLoader.LoadStages();
        MissionDefinition[] missions = missionCatalogLoader.LoadMissions();

        currencySystem.Initialize(state, saveManager);
        upgradeSystem.Initialize(state, currencySystem, upgrades);
        offlineProgressSystem.Initialize(state, currencySystem);
        dailyRewardSystem.Initialize(state, currencySystem, saveManager);
        boostSystem.Initialize(currencySystem);
        stageSystem.Initialize(currencySystem, stages);
        missionSystem.Initialize(state, currencySystem, saveManager, missions);
        adsManager.Initialize(currencySystem);
        iapManager.Initialize(currencySystem);
        analyticsManager.TrackGameStarted();
    }

    private void AutoWireSystems()
    {
        if (saveManager == null) saveManager = GetComponent<SaveManager>();
        if (currencySystem == null) currencySystem = GetComponent<CurrencySystem>();
        if (upgradeCatalogLoader == null) upgradeCatalogLoader = GetComponent<UpgradeCatalogLoader>();
        if (upgradeSystem == null) upgradeSystem = GetComponent<UpgradeSystem>();
        if (offlineProgressSystem == null) offlineProgressSystem = GetComponent<OfflineProgressSystem>();
        if (dailyRewardSystem == null) dailyRewardSystem = GetComponent<DailyRewardSystem>();
        if (boostSystem == null) boostSystem = GetComponent<BoostSystem>();
        if (stageCatalogLoader == null) stageCatalogLoader = GetComponent<StageCatalogLoader>();
        if (stageSystem == null) stageSystem = GetComponent<StageSystem>();
        if (missionCatalogLoader == null) missionCatalogLoader = GetComponent<MissionCatalogLoader>();
        if (missionSystem == null) missionSystem = GetComponent<MissionSystem>();
        if (adsManager == null) adsManager = GetComponent<AdsManager>();
        if (iapManager == null) iapManager = GetComponent<IAPManager>();
        if (analyticsManager == null) analyticsManager = GetComponent<AnalyticsManager>();
    }

    private void SubscribeToEvents()
    {
        GameEvents.UpgradeLevelChanged += OnUpgradeLevelChanged;
        GameEvents.OfflineRewardCalculated += OnOfflineRewardCalculated;
    }

    private void UnsubscribeFromEvents()
    {
        GameEvents.UpgradeLevelChanged -= OnUpgradeLevelChanged;
        GameEvents.OfflineRewardCalculated -= OnOfflineRewardCalculated;
    }

    private void OnUpgradeLevelChanged(string upgradeId, int level)
    {
        analyticsManager.TrackUpgradePurchased(upgradeId, level);
        saveManager.Save(state);
    }

    private void OnOfflineRewardCalculated(double reward)
    {
        analyticsManager.TrackOfflineRewardClaimed(reward);
    }
}
