using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class PrototypeSceneBuilder
{
    private const string ScenePath = "Assets/Scenes/Game.unity";
    private const string UpgradeButtonPrefabPath = "Assets/Prefabs/UI/UpgradeButtonView.prefab";
    private const string MissionRowPrefabPath = "Assets/Prefabs/UI/MissionRowView.prefab";
    private const string FloatingTextPrefabPath = "Assets/Prefabs/UI/FloatingText.prefab";

    [MenuItem("Tools/Cosmic Crystal Factory/Create Prototype Scene")]
    public static void CreatePrototypeScene()
    {
        EnsureDirectories();
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "Game";

        CreateCamera();
        CreateEventSystem();
        CreateGameManager();

        Canvas canvas = CreateCanvas();
        Image backgroundImage = CreateBackground(canvas.transform);
        CreateDimOverlay(canvas.transform);
        CreateAmbientGlow("TopAmbientGlow", canvas.transform, new Vector2(-0.18f, 0.7f), new Vector2(0.82f, 1.16f), new Color(0.1f, 0.7f, 1f, 0.18f));
        CreateAmbientGlow("BottomAmbientGlow", canvas.transform, new Vector2(0.12f, -0.12f), new Vector2(1.18f, 0.36f), new Color(0.45f, 0.14f, 0.9f, 0.12f));
        Image energyBandImage = CreateEnergyBand(canvas.transform);
        GameObject phoneRoot = CreatePhoneRoot(canvas.transform);
        GameObject mainLayout = CreateMainLayout(phoneRoot.transform);

        GameObject topPanel = CreateContainerPanel("TopHudPanel", mainLayout.transform, 276, new Color(0.035f, 0.055f, 0.09f, 0.86f));
        TMP_Text titleText = CreateText("GameTitleText", topPanel.transform, "ESTACION AURORA", 40, 46, TextAlignmentOptions.Center);
        titleText.color = new Color(0.72f, 0.96f, 1f);
        TMP_Text stageTitleText = CreateText("StageTitleText", topPanel.transform, "Sector 1: Nucleo Dormido", 22, 28, TextAlignmentOptions.Center);
        TMP_Text loreText = CreateText("LoreText", topPanel.transform, "Produce energia, cumple pedidos y restaura sectores para convertir Aurora en negocio.", 18, 48, TextAlignmentOptions.Center);
        loreText.color = new Color(0.85f, 0.9f, 0.96f);

        GameObject statsRow = CreateHorizontalGroup("StatsRow", topPanel.transform, 94);
        TMP_Text coinsText = CreateMetricBlock("CoinsMetric", statsRow.transform, "CRISTALES", "0", 34);
        TMP_Text cpsText = CreateMetricBlock("CpsMetric", statsRow.transform, "AUTO", "0/s", 24);
        TMP_Text clickPowerText = CreateMetricBlock("ClickMetric", statsRow.transform, "TOQUE", "+1", 24);

        Button crystalButton = CreateCrystalButton(mainLayout.transform);

        GameObject statusPanel = CreateContainerPanel("ObjectivePanel", mainLayout.transform, 82, new Color(0.11f, 0.08f, 0.02f, 0.88f));
        TMP_Text statusText = CreateText("StatusText", statusPanel.transform, "Pedido activo: despierta el nucleo con 10 toques y compra Pulidor Manual", 22, 58, TextAlignmentOptions.Center);
        statusText.color = new Color(1f, 0.9f, 0.48f);

        GameObject actionRow = CreateHorizontalGroup("ActionRow", mainLayout.transform, 72);
        Button dailyRewardButton = CreateButton("DailyRewardButton", actionRow.transform, "Recompensa", new Color(0.45f, 0.31f, 0.08f), 20, 72);
        Button rewardedAdButton = CreateButton("RewardedAdButton", actionRow.transform, "Boost x2", new Color(0.08f, 0.35f, 0.25f), 20, 72);
        Button storeButton = CreateButton("OpenStoreButton", actionRow.transform, "Tienda", new Color(0.18f, 0.15f, 0.36f), 20, 72);

        GameObject listsColumn = CreateVerticalGroup("ListsColumn", mainLayout.transform, 820, 12);
        GameObject shopPanel = CreatePanel("ShopPanel", listsColumn.transform, "MEJORAS", 510);
        Transform shopContent = CreateScrollContent(shopPanel.transform, "ShopScrollView", 436);

        GameObject missionsPanel = CreatePanel("MissionsPanel", listsColumn.transform, "MISIONES", 298);
        Transform missionsContent = CreateScrollContent(missionsPanel.transform, "MissionsScrollView", 224);

        UpgradeButtonView upgradePrefab = CreateUpgradeButtonPrefab();
        MissionRowView missionPrefab = CreateMissionRowPrefab();
        TMP_Text floatingTextPrefab = CreateFloatingTextPrefab();

        HUDController hud = canvas.gameObject.AddComponent<HUDController>();
        SetObjectReference(hud, "coinsText", coinsText);
        SetObjectReference(hud, "coinsPerSecondText", cpsText);
        SetObjectReference(hud, "clickPowerText", clickPowerText);
        SetObjectReference(hud, "statusText", statusText);
        SetObjectReference(hud, "crystalButton", crystalButton);
        SetObjectReference(hud, "dailyRewardButton", dailyRewardButton);
        SetObjectReference(hud, "rewardedAdButton", rewardedAdButton);

        StageDisplayController stageDisplay = canvas.gameObject.AddComponent<StageDisplayController>();
        SetObjectReference(stageDisplay, "stageTitleText", stageTitleText);
        SetObjectReference(stageDisplay, "loreText", loreText);
        SetObjectReference(stageDisplay, "backgroundImage", backgroundImage);
        SetObjectReference(stageDisplay, "accentImage", energyBandImage);

        FloatingTextSpawner floatingTextSpawner = crystalButton.gameObject.AddComponent<FloatingTextSpawner>();
        SetObjectReference(floatingTextSpawner, "floatingTextPrefab", floatingTextPrefab);
        SetObjectReference(floatingTextSpawner, "spawnParent", crystalButton.transform);

        ShopController shop = shopPanel.AddComponent<ShopController>();
        SetObjectReference(shop, "container", shopContent);
        SetObjectReference(shop, "upgradeButtonPrefab", upgradePrefab);

        MissionPanelController missions = missionsPanel.AddComponent<MissionPanelController>();
        SetObjectReference(missions, "container", missionsContent);
        SetObjectReference(missions, "missionRowPrefab", missionPrefab);

        GameObject mainMenuRoot = CreateMainMenu(canvas.transform, out Button startButton, out Button storyButton);
        GameObject loadingRoot = CreateLoadingScreen(canvas.transform, out TMP_Text loadingText);
        GameObject storyRoot = CreateStoryScreen(canvas.transform, out Button storyBackButton);
        GameObject storeRoot = CreateStoreScreen(canvas.transform, out Button closeStoreButton, out TMP_Text storeStatusText, out Button smallPackButton, out Button mediumPackButton, out Button removeAdsButton);
        CreateRewardPopup(canvas.transform);

        GameFlowController flow = canvas.gameObject.AddComponent<GameFlowController>();
        SetObjectReference(flow, "gameplayRoot", mainLayout);
        SetObjectReference(flow, "mainMenuRoot", mainMenuRoot);
        SetObjectReference(flow, "loadingRoot", loadingRoot);
        SetObjectReference(flow, "storyRoot", storyRoot);
        SetObjectReference(flow, "storeRoot", storeRoot);
        SetObjectReference(flow, "loadingText", loadingText);
        SetObjectReference(flow, "storeStatusText", storeStatusText);
        SetObjectReference(flow, "startButton", startButton);
        SetObjectReference(flow, "storyButton", storyButton);
        SetObjectReference(flow, "storyBackButton", storyBackButton);
        SetObjectReference(flow, "openStoreButton", storeButton);
        SetObjectReference(flow, "closeStoreButton", closeStoreButton);
        SetObjectReference(flow, "smallPackButton", smallPackButton);
        SetObjectReference(flow, "mediumPackButton", mediumPackButton);
        SetObjectReference(flow, "removeAdsButton", removeAdsButton);

        EditorSceneManager.SaveScene(scene, ScenePath);
        Selection.activeObject = canvas.gameObject;
        Debug.Log($"Mobile prototype scene created at {ScenePath}");
    }

    private static void EnsureDirectories()
    {
        Directory.CreateDirectory("Assets/Scenes");
        Directory.CreateDirectory("Assets/Prefabs/UI");
    }

    private static void CreateCamera()
    {
        GameObject cameraObject = new GameObject("Main Camera");
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.tag = "MainCamera";
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.04f, 0.06f, 0.1f);
        camera.orthographic = true;
        camera.orthographicSize = 5;
        cameraObject.transform.position = new Vector3(0, 0, -10);
    }

    private static void CreateEventSystem()
    {
        GameObject eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<StandaloneInputModule>();
    }

    private static void CreateGameManager()
    {
        GameObject gameManager = new GameObject("GameManager");
        gameManager.AddComponent<SaveManager>();
        gameManager.AddComponent<CurrencySystem>();
        gameManager.AddComponent<UpgradeCatalogLoader>();
        gameManager.AddComponent<UpgradeSystem>();
        gameManager.AddComponent<OfflineProgressSystem>();
        gameManager.AddComponent<DailyRewardSystem>();
        gameManager.AddComponent<BoostSystem>();
        gameManager.AddComponent<StageCatalogLoader>();
        gameManager.AddComponent<StageSystem>();
        gameManager.AddComponent<MissionCatalogLoader>();
        gameManager.AddComponent<MissionSystem>();
        gameManager.AddComponent<AdsManager>();
        gameManager.AddComponent<IAPManager>();
        gameManager.AddComponent<AnalyticsManager>();
        gameManager.AddComponent<GameManager>();
    }

    private static Canvas CreateCanvas()
    {
        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        scaler.matchWidthOrHeight = 1f;

        canvasObject.AddComponent<GraphicRaycaster>();
        return canvas;
    }

    private static Image CreateBackground(Transform parent)
    {
        GameObject backgroundObject = new GameObject("DynamicBackground");
        backgroundObject.transform.SetParent(parent, false);
        Image image = backgroundObject.AddComponent<Image>();
        Sprite backgroundSprite = Resources.Load<Sprite>("Art/Backgrounds/bg_sleeping_fragment");
        image.sprite = backgroundSprite;
        image.color = backgroundSprite == null ? new Color(0.06f, 0.09f, 0.15f, 1f) : Color.white;
        image.preserveAspect = false;
        image.raycastTarget = false;

        RectTransform rect = backgroundObject.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        return image;
    }

    private static Image CreateEnergyBand(Transform parent)
    {
        GameObject bandObject = new GameObject("PulsingEnergyBand");
        bandObject.transform.SetParent(parent, false);
        Image image = bandObject.AddComponent<Image>();
        image.color = new Color(0.15f, 0.86f, 1f, 0.16f);
        image.raycastTarget = false;
        bandObject.AddComponent<AmbientPulseController>();

        RectTransform rect = bandObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0.47f);
        rect.anchorMax = new Vector2(1, 0.58f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        return image;
    }

    private static void CreateDimOverlay(Transform parent)
    {
        GameObject overlayObject = new GameObject("ReadabilityDimOverlay");
        overlayObject.transform.SetParent(parent, false);
        Image image = overlayObject.AddComponent<Image>();
        image.color = new Color(0f, 0.01f, 0.025f, 0.42f);
        image.raycastTarget = false;

        RectTransform rect = overlayObject.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static void CreateAmbientGlow(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Color centerColor)
    {
        GameObject glowObject = CreateEllipse(name, parent, centerColor, new Color(centerColor.r, centerColor.g, centerColor.b, 0f));
        RectTransform rect = glowObject.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static GameObject CreatePhoneRoot(Transform parent)
    {
        GameObject phoneRoot = new GameObject("PhoneRoot_9x16");
        phoneRoot.transform.SetParent(parent, false);

        RectTransform rect = phoneRoot.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(980, 1820);

        AspectRatioFitter aspect = phoneRoot.AddComponent<AspectRatioFitter>();
        aspect.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        aspect.aspectRatio = 9f / 16f;

        return phoneRoot;
    }

    private static GameObject CreateMainLayout(Transform parent)
    {
        GameObject layoutObject = new GameObject("MainLayout");
        layoutObject.transform.SetParent(parent, false);

        RectTransform rect = layoutObject.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = new Vector2(42, 36);
        rect.offsetMax = new Vector2(-42, -36);

        VerticalLayoutGroup layout = layoutObject.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 8;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlHeight = false;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = true;

        return layoutObject;
    }

    private static GameObject CreateHorizontalGroup(string name, Transform parent, float height)
    {
        GameObject group = new GameObject(name);
        group.transform.SetParent(parent, false);
        RectTransform rect = group.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(0, height);
        AddLayoutElement(group, height);

        HorizontalLayoutGroup layout = group.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 14;
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = true;
        layout.childForceExpandWidth = true;

        return group;
    }

    private static GameObject CreateVerticalGroup(string name, Transform parent, float height, float spacing)
    {
        GameObject group = new GameObject(name);
        group.transform.SetParent(parent, false);
        RectTransform rect = group.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(0, height);
        AddLayoutElement(group, height);

        VerticalLayoutGroup layout = group.AddComponent<VerticalLayoutGroup>();
        layout.spacing = spacing;
        layout.childControlWidth = true;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        return group;
    }

    private static Button CreateCrystalButton(Transform parent)
    {
        GameObject buttonObject = new GameObject("CrystalButton");
        buttonObject.transform.SetParent(parent, false);
        RectTransform rect = buttonObject.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(0, 342);
        AddLayoutElement(buttonObject, 342);

        Image hitArea = buttonObject.AddComponent<Image>();
        hitArea.color = new Color(1f, 1f, 1f, 0.001f);
        hitArea.raycastTarget = true;
        Button button = buttonObject.AddComponent<Button>();
        button.targetGraphic = hitArea;
        CrystalPulseController pulse = buttonObject.AddComponent<CrystalPulseController>();

        GameObject outerGlowObject = CreateEllipse("OuterCrystalGlow", buttonObject.transform, new Color(0.13f, 0.86f, 1f, 0.32f), new Color(0.02f, 0.12f, 0.28f, 0f));
        RectTransform outerGlowRect = outerGlowObject.GetComponent<RectTransform>();
        outerGlowRect.anchorMin = new Vector2(0.04f, 0.03f);
        outerGlowRect.anchorMax = new Vector2(0.96f, 0.97f);

        GameObject innerGlowObject = CreateEllipse("InnerCrystalGlow", buttonObject.transform, new Color(0.8f, 1f, 1f, 0.42f), new Color(0.05f, 0.5f, 0.9f, 0f));
        RectTransform innerGlowRect = innerGlowObject.GetComponent<RectTransform>();
        innerGlowRect.anchorMin = new Vector2(0.16f, 0.12f);
        innerGlowRect.anchorMax = new Vector2(0.84f, 0.88f);

        GameObject pedestalObject = new GameObject("CrystalPedestal");
        pedestalObject.transform.SetParent(buttonObject.transform, false);
        Image pedestal = pedestalObject.AddComponent<Image>();
        pedestal.color = new Color(0.08f, 0.2f, 0.3f, 0.88f);
        pedestal.raycastTarget = false;
        RectTransform pedestalRect = pedestalObject.GetComponent<RectTransform>();
        pedestalRect.anchorMin = new Vector2(0.3f, 0.04f);
        pedestalRect.anchorMax = new Vector2(0.7f, 0.14f);
        pedestalRect.offsetMin = Vector2.zero;
        pedestalRect.offsetMax = Vector2.zero;

        GameObject crystalObject = new GameObject("CrystalGem");
        crystalObject.transform.SetParent(buttonObject.transform, false);
        CrystalGraphic crystalGraphic = crystalObject.AddComponent<CrystalGraphic>();
        crystalGraphic.raycastTarget = false;

        RectTransform crystalRect = crystalObject.GetComponent<RectTransform>();
        crystalRect.anchorMin = new Vector2(0.29f, 0.13f);
        crystalRect.anchorMax = new Vector2(0.71f, 0.86f);
        crystalRect.offsetMin = Vector2.zero;
        crystalRect.offsetMax = Vector2.zero;

        TMP_Text tapLabel = CreateText("TapLabel", buttonObject.transform, "TOCAR CRISTAL", 24, 44, TextAlignmentOptions.Center);
        tapLabel.color = new Color(1f, 0.93f, 0.52f);
        RectTransform tapLabelRect = tapLabel.GetComponent<RectTransform>();
        tapLabelRect.anchorMin = new Vector2(0.18f, 0.02f);
        tapLabelRect.anchorMax = new Vector2(0.82f, 0.16f);
        tapLabelRect.offsetMin = Vector2.zero;
        tapLabelRect.offsetMax = Vector2.zero;

        SetObjectReference(pulse, "target", crystalObject.transform);
        return button;
    }

    private static GameObject CreateContainerPanel(string name, Transform parent, float height, Color color)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);
        Image image = panel.AddComponent<Image>();
        image.color = color;
        image.raycastTarget = false;
        AddLayoutElement(panel, height);

        VerticalLayoutGroup layout = panel.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(18, 18, 12, 12);
        layout.spacing = 5;
        layout.childControlWidth = true;
        layout.childForceExpandWidth = true;
        layout.childControlHeight = false;
        layout.childForceExpandHeight = false;
        return panel;
    }

    private static TMP_Text CreateMetricBlock(string name, Transform parent, string label, string value, int valueSize)
    {
        GameObject block = CreateContainerPanel(name, parent, 94, new Color(0.02f, 0.09f, 0.13f, 0.9f));
        TMP_Text labelText = CreateText("Label", block.transform, label, 13, 22, TextAlignmentOptions.Center);
        labelText.color = new Color(0.62f, 0.82f, 0.88f);
        TMP_Text valueText = CreateText("Value", block.transform, value, valueSize, 44, TextAlignmentOptions.Center);
        valueText.color = Color.white;
        return valueText;
    }

    private static GameObject CreateEllipse(string name, Transform parent, Color centerColor, Color edgeColor)
    {
        GameObject ellipseObject = new GameObject(name);
        ellipseObject.transform.SetParent(parent, false);
        SoftEllipseGraphic ellipse = ellipseObject.AddComponent<SoftEllipseGraphic>();
        SetColorReference(ellipse, "centerColor", centerColor);
        SetColorReference(ellipse, "edgeColor", edgeColor);
        ellipse.raycastTarget = false;
        ellipseObject.AddComponent<AmbientPulseController>();

        RectTransform rect = ellipseObject.GetComponent<RectTransform>();
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        return ellipseObject;
    }

    private static GameObject CreatePanel(string name, Transform parent, string title, float height)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);
        Image image = panel.AddComponent<Image>();
        image.color = new Color(0.07f, 0.09f, 0.14f, 0.94f);
        image.raycastTarget = false;
        AddLayoutElement(panel, height);

        VerticalLayoutGroup layout = panel.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(16, 16, 14, 14);
        layout.spacing = 10;
        layout.childControlWidth = true;
        layout.childForceExpandWidth = true;
        layout.childControlHeight = false;
        layout.childForceExpandHeight = false;

        TMP_Text titleText = CreateText(title + "Title", panel.transform, title, 20, 34, TextAlignmentOptions.Center);
        titleText.color = new Color(0.9f, 0.96f, 1f);
        return panel;
    }

    private static GameObject CreateMainMenu(Transform parent, out Button startButton, out Button storyButton)
    {
        GameObject root = CreateFullScreenPanel("MainMenu", parent, new Color(0.015f, 0.035f, 0.06f, 0.96f));
        VerticalLayoutGroup layout = root.GetComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(54, 54, 170, 120);
        layout.spacing = 22;

        TMP_Text title = CreateText("Title", root.transform, "ESTACION AURORA", 54, 72, TextAlignmentOptions.Center);
        title.color = new Color(0.72f, 0.96f, 1f);
        TMP_Text subtitle = CreateText("Subtitle", root.transform, "Una fabrica de cristales, una estacion apagada y una galaxia esperando energia.", 24, 96, TextAlignmentOptions.Center);
        subtitle.color = new Color(0.82f, 0.9f, 0.95f);

        GameObject hookPanel = CreateContainerPanel("HookPanel", root.transform, 260, new Color(0.04f, 0.1f, 0.14f, 0.92f));
        TMP_Text hookTitle = CreateText("HookTitle", hookPanel.transform, "TU MISION", 24, 36, TextAlignmentOptions.Center);
        hookTitle.color = new Color(1f, 0.88f, 0.45f);
        CreateText("HookCopy", hookPanel.transform, "Reactivar sectores, cumplir pedidos de energia, mejorar maquinas y convertir Aurora en la red energetica mas rentable del espacio.", 23, 150, TextAlignmentOptions.Center);

        startButton = CreateButton("StartButton", root.transform, "INICIAR FABRICA", new Color(0.06f, 0.58f, 0.78f), 26, 92);
        storyButton = CreateButton("StoryButton", root.transform, "VER HISTORIA", new Color(0.12f, 0.15f, 0.24f), 22, 78);
        return root;
    }

    private static GameObject CreateLoadingScreen(Transform parent, out TMP_Text loadingText)
    {
        GameObject root = CreateFullScreenPanel("LoadingScreen", parent, new Color(0.01f, 0.025f, 0.045f, 0.98f));
        VerticalLayoutGroup layout = root.GetComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(56, 56, 390, 120);
        layout.spacing = 20;

        TMP_Text title = CreateText("LoadingTitle", root.transform, "AURORA", 52, 74, TextAlignmentOptions.Center);
        title.color = new Color(0.72f, 0.96f, 1f);
        loadingText = CreateText("LoadingText", root.transform, "Calibrando nucleo de cristal...", 26, 90, TextAlignmentOptions.Center);
        loadingText.color = new Color(1f, 0.9f, 0.48f);
        CreateText("Hint", root.transform, "Consejo: reinvierte tus cristales antes de usar boosts para multiplicar mejor.", 20, 96, TextAlignmentOptions.Center);
        root.SetActive(false);
        return root;
    }

    private static GameObject CreateStoryScreen(Transform parent, out Button backButton)
    {
        GameObject root = CreateFullScreenPanel("StoryScreen", parent, new Color(0.015f, 0.025f, 0.045f, 0.98f));
        VerticalLayoutGroup layout = root.GetComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(50, 50, 110, 90);
        layout.spacing = 18;

        TMP_Text title = CreateText("StoryTitle", root.transform, "LA RAZON", 42, 60, TextAlignmentOptions.Center);
        title.color = new Color(0.72f, 0.96f, 1f);
        CreateText("StoryBody", root.transform, "La Estacion Aurora era una fabrica orbital que alimentaba colonias enteras. Un fallo dejo sus sectores dormidos y solo queda un nucleo de cristal capaz de reiniciarla. Cada toque produce energia. Cada mejora automatiza una parte. Cada pedido completado recupera un sector y abre una oportunidad de negocio mayor.", 24, 390, TextAlignmentOptions.Center);
        CreateText("LoopBody", root.transform, "Loop: tocar -> comprar mejoras -> cumplir pedidos -> desbloquear sectores -> usar boosts opcionales -> volver por recompensas diarias.", 22, 150, TextAlignmentOptions.Center);
        backButton = CreateButton("BackButton", root.transform, "VOLVER", new Color(0.08f, 0.28f, 0.36f), 22, 78);
        root.SetActive(false);
        return root;
    }

    private static GameObject CreateStoreScreen(Transform parent, out Button closeButton, out TMP_Text storeStatusText, out Button smallPackButton, out Button mediumPackButton, out Button removeAdsButton)
    {
        GameObject root = CreateFullScreenPanel("StoreScreen", parent, new Color(0.01f, 0.02f, 0.04f, 0.94f));
        VerticalLayoutGroup layout = root.GetComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(48, 48, 140, 100);
        layout.spacing = 16;

        TMP_Text title = CreateText("StoreTitle", root.transform, "TIENDA DE APOYO", 40, 58, TextAlignmentOptions.Center);
        title.color = new Color(0.72f, 0.96f, 1f);
        CreateText("StoreCopy", root.transform, "Monetizacion pensada para sostener el juego: boosts voluntarios, packs de avance y Remove Ads. Nada bloquea la historia principal.", 22, 120, TextAlignmentOptions.Center);

        smallPackButton = CreateButton("SmallPackButton", root.transform, "Pack 1K cristales", new Color(0.08f, 0.32f, 0.42f), 22, 76);
        mediumPackButton = CreateButton("MediumPackButton", root.transform, "Pack 6K cristales", new Color(0.08f, 0.38f, 0.45f), 22, 76);
        removeAdsButton = CreateButton("RemoveAdsButton", root.transform, "Remove Ads", new Color(0.42f, 0.29f, 0.08f), 22, 76);
        storeStatusText = CreateText("StoreStatus", root.transform, "Compras simuladas para probar economia y conversion.", 20, 80, TextAlignmentOptions.Center);
        storeStatusText.color = new Color(1f, 0.9f, 0.48f);
        closeButton = CreateButton("CloseStoreButton", root.transform, "CERRAR", new Color(0.12f, 0.15f, 0.24f), 22, 76);
        root.SetActive(false);
        return root;
    }

    private static PopupController CreateRewardPopup(Transform parent)
    {
        GameObject root = CreateFullScreenPanel("RewardPopup", parent, new Color(0f, 0.01f, 0.02f, 0.68f));
        VerticalLayoutGroup rootLayout = root.GetComponent<VerticalLayoutGroup>();
        rootLayout.padding = new RectOffset(70, 70, 560, 0);
        rootLayout.spacing = 12;

        GameObject card = CreateContainerPanel("RewardCard", root.transform, 360, new Color(0.045f, 0.08f, 0.12f, 0.98f));
        TMP_Text titleText = CreateText("TitleText", card.transform, "Recompensa", 34, 58, TextAlignmentOptions.Center);
        titleText.color = new Color(1f, 0.9f, 0.48f);
        TMP_Text messageText = CreateText("MessageText", card.transform, "Aurora recibio nuevos recursos.", 24, 150, TextAlignmentOptions.Center);
        Button closeButton = CreateButton("CloseButton", card.transform, "CONTINUAR", new Color(0.06f, 0.58f, 0.78f), 22, 76);

        PopupController popup = parent.gameObject.AddComponent<PopupController>();
        SetObjectReference(popup, "root", root);
        SetObjectReference(popup, "titleText", titleText);
        SetObjectReference(popup, "messageText", messageText);
        SetObjectReference(popup, "closeButton", closeButton);
        root.SetActive(false);
        return popup;
    }

    private static GameObject CreateFullScreenPanel(string name, Transform parent, Color color)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent, false);
        Image image = root.AddComponent<Image>();
        image.color = color;
        image.raycastTarget = true;

        RectTransform rect = root.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        VerticalLayoutGroup layout = root.AddComponent<VerticalLayoutGroup>();
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlWidth = true;
        layout.childForceExpandWidth = true;
        layout.childControlHeight = false;
        layout.childForceExpandHeight = false;
        return root;
    }

    private static Transform CreateScrollContent(Transform parent, string name, float height)
    {
        GameObject scrollObject = new GameObject(name);
        scrollObject.transform.SetParent(parent, false);
        RectTransform scrollRectTransform = scrollObject.AddComponent<RectTransform>();
        scrollRectTransform.sizeDelta = new Vector2(0, height);
        AddLayoutElement(scrollObject, height);

        Image background = scrollObject.AddComponent<Image>();
        background.color = new Color(0.04f, 0.05f, 0.08f, 0.74f);

        ScrollRect scrollRect = scrollObject.AddComponent<ScrollRect>();

        GameObject viewport = new GameObject("Viewport");
        viewport.transform.SetParent(scrollObject.transform, false);
        RectTransform viewportRect = viewport.AddComponent<RectTransform>();
        viewportRect.anchorMin = Vector2.zero;
        viewportRect.anchorMax = Vector2.one;
        viewportRect.offsetMin = Vector2.zero;
        viewportRect.offsetMax = Vector2.zero;
        viewport.AddComponent<Mask>().showMaskGraphic = false;
        Image viewportImage = viewport.AddComponent<Image>();
        viewportImage.color = Color.white;

        GameObject content = new GameObject("Content");
        content.transform.SetParent(viewport.transform, false);
        RectTransform contentRect = content.AddComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0, 1);
        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.pivot = new Vector2(0.5f, 1);
        contentRect.anchoredPosition = Vector2.zero;
        contentRect.sizeDelta = new Vector2(0, 0);

        VerticalLayoutGroup contentLayout = content.AddComponent<VerticalLayoutGroup>();
        contentLayout.spacing = 9;
        contentLayout.padding = new RectOffset(8, 8, 8, 8);
        contentLayout.childControlWidth = true;
        contentLayout.childControlHeight = false;
        contentLayout.childForceExpandWidth = true;
        contentLayout.childForceExpandHeight = false;
        content.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        scrollRect.viewport = viewportRect;
        scrollRect.content = contentRect;
        scrollRect.horizontal = false;
        scrollRect.vertical = true;

        return content.transform;
    }

    private static TMP_Text CreateText(string name, Transform parent, string text, int size, float height, TextAlignmentOptions alignment)
    {
        GameObject textObject = new GameObject(name);
        textObject.transform.SetParent(parent, false);
        TextMeshProUGUI tmp = textObject.AddComponent<TextMeshProUGUI>();
        tmp.raycastTarget = false;
        tmp.text = text;
        tmp.fontSize = size;
        tmp.enableAutoSizing = true;
        tmp.fontSizeMin = Mathf.Max(10, size - 8);
        tmp.fontSizeMax = size;
        tmp.alignment = alignment;
        tmp.color = Color.white;
        tmp.margin = new Vector4(8, 0, 8, 0);

        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(0, height);
        AddLayoutElement(textObject, height);
        return tmp;
    }

    private static Button CreateButton(string name, Transform parent, string label, Color color, int fontSize, float height)
    {
        GameObject buttonObject = new GameObject(name);
        buttonObject.transform.SetParent(parent, false);
        Image image = buttonObject.AddComponent<Image>();
        image.color = color;
        Button button = buttonObject.AddComponent<Button>();
        button.targetGraphic = image;

        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(0, height);
        AddLayoutElement(buttonObject, height);

        TMP_Text text = CreateText("Label", buttonObject.transform, label, fontSize, height, TextAlignmentOptions.Center);
        text.raycastTarget = false;
        RectTransform textRect = text.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        return button;
    }

    private static UpgradeButtonView CreateUpgradeButtonPrefab()
    {
        GameObject root = CreateRowBase("UpgradeButtonView", 132, true);
        UpgradeButtonView view = root.AddComponent<UpgradeButtonView>();
        HorizontalLayoutGroup rowLayout = root.GetComponent<HorizontalLayoutGroup>();
        rowLayout.padding = new RectOffset(12, 12, 10, 10);
        rowLayout.spacing = 12;

        Image iconImage = CreateIconImage("IconImage", root.transform, 88, 88);
        GameObject textColumn = CreateVerticalGroup("TextColumn", root.transform, 112, 3);
        TMP_Text nameText = CreateText("NameText", textColumn.transform, "Upgrade", 20, 30, TextAlignmentOptions.Left);
        TMP_Text descriptionText = CreateText("DescriptionText", textColumn.transform, "Description", 15, 38, TextAlignmentOptions.Left);
        TMP_Text levelText = CreateText("LevelText", textColumn.transform, "Nivel 0", 14, 22, TextAlignmentOptions.Left);
        TMP_Text costText = CreateText("CostText", textColumn.transform, "0", 16, 22, TextAlignmentOptions.Left);
        Button buyButton = CreateButton("BuyButton", root.transform, "Comprar", new Color(0.1f, 0.65f, 0.9f), 16, 88);
        buyButton.GetComponent<LayoutElement>().preferredWidth = 160;
        buyButton.GetComponent<LayoutElement>().flexibleWidth = 0;

        SetObjectReference(view, "nameText", nameText);
        SetObjectReference(view, "descriptionText", descriptionText);
        SetObjectReference(view, "levelText", levelText);
        SetObjectReference(view, "costText", costText);
        SetObjectReference(view, "iconImage", iconImage);
        SetObjectReference(view, "buyButton", buyButton);

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, UpgradeButtonPrefabPath);
        Object.DestroyImmediate(root);
        return prefab.GetComponent<UpgradeButtonView>();
    }

    private static MissionRowView CreateMissionRowPrefab()
    {
        GameObject root = CreateRowBase("MissionRowView", 172, false);
        MissionRowView view = root.AddComponent<MissionRowView>();
        TMP_Text titleText = CreateText("TitleText", root.transform, "Mission", 18, 28, TextAlignmentOptions.Left);
        TMP_Text descriptionText = CreateText("DescriptionText", root.transform, "Description", 14, 38, TextAlignmentOptions.Left);
        TMP_Text progressText = CreateText("ProgressText", root.transform, "0 / 0", 14, 24, TextAlignmentOptions.Left);
        TMP_Text rewardText = CreateText("RewardText", root.transform, "+0", 16, 24, TextAlignmentOptions.Left);
        Button claimButton = CreateButton("ClaimButton", root.transform, "Reclamar", new Color(0.95f, 0.72f, 0.18f), 16, 42);

        SetObjectReference(view, "titleText", titleText);
        SetObjectReference(view, "descriptionText", descriptionText);
        SetObjectReference(view, "progressText", progressText);
        SetObjectReference(view, "rewardText", rewardText);
        SetObjectReference(view, "claimButton", claimButton);

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, MissionRowPrefabPath);
        Object.DestroyImmediate(root);
        return prefab.GetComponent<MissionRowView>();
    }

    private static TMP_Text CreateFloatingTextPrefab()
    {
        GameObject root = new GameObject("FloatingText");
        TextMeshProUGUI text = root.AddComponent<TextMeshProUGUI>();
        text.text = "+1";
        text.fontSize = 30;
        text.alignment = TextAlignmentOptions.Center;
        text.color = new Color(1f, 0.91f, 0.42f, 1f);

        RectTransform rect = root.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(260, 60);

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, FloatingTextPrefabPath);
        Object.DestroyImmediate(root);
        return prefab.GetComponent<TMP_Text>();
    }

    private static Image CreateIconImage(string name, Transform parent, float width, float height)
    {
        GameObject iconObject = new GameObject(name);
        iconObject.transform.SetParent(parent, false);
        Image image = iconObject.AddComponent<Image>();
        image.color = Color.white;
        image.preserveAspect = true;
        image.raycastTarget = false;

        RectTransform rect = iconObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(width, height);
        AddLayoutElement(iconObject, height);
        iconObject.GetComponent<LayoutElement>().preferredWidth = width;
        iconObject.GetComponent<LayoutElement>().flexibleWidth = 0;
        return image;
    }

    private static GameObject CreateRowBase(string name, float height, bool horizontal)
    {
        GameObject root = new GameObject(name);
        Image image = root.AddComponent<Image>();
        image.color = new Color(0.12f, 0.15f, 0.22f, 1f);
        image.raycastTarget = false;

        if (horizontal)
        {
            HorizontalLayoutGroup layout = root.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(12, 12, 10, 10);
            layout.spacing = 10;
            layout.childControlWidth = true;
            layout.childForceExpandWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandHeight = true;
        }
        else
        {
            VerticalLayoutGroup layout = root.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(12, 12, 10, 10);
            layout.spacing = 4;
            layout.childControlWidth = true;
            layout.childForceExpandWidth = true;
            layout.childControlHeight = false;
            layout.childForceExpandHeight = false;
        }

        AddLayoutElement(root, height);
        return root;
    }

    private static void AddLayoutElement(GameObject target, float preferredHeight)
    {
        LayoutElement element = target.GetComponent<LayoutElement>();
        if (element == null)
        {
            element = target.AddComponent<LayoutElement>();
        }

        element.preferredHeight = preferredHeight;
        element.flexibleWidth = 1;
    }

    private static void SetObjectReference(Object target, string propertyName, Object value)
    {
        SerializedObject serializedObject = new SerializedObject(target);
        SerializedProperty property = serializedObject.FindProperty(propertyName);

        if (property == null)
        {
            Debug.LogWarning($"Property {propertyName} not found on {target.name}");
            return;
        }

        property.objectReferenceValue = value;
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void SetColorReference(Object target, string propertyName, Color value)
    {
        SerializedObject serializedObject = new SerializedObject(target);
        SerializedProperty property = serializedObject.FindProperty(propertyName);

        if (property == null)
        {
            Debug.LogWarning($"Property {propertyName} not found on {target.name}");
            return;
        }

        property.colorValue = value;
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }
}
