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

        CreateEventSystem();
        CreateGameManager();

        Canvas canvas = CreateCanvas();
    Image backgroundImage = CreateBackground(canvas.transform);
    Image energyBandImage = CreateEnergyBand(canvas.transform);
        GameObject mainLayout = CreateMainLayout(canvas.transform);

    TMP_Text titleText = CreateText("GameTitleText", mainLayout.transform, "CRYSTAL FACTORY", 34, TextAlignmentOptions.Center);
    titleText.color = new Color(0.72f, 0.96f, 1f);
    TMP_Text stageTitleText = CreateText("StageTitleText", mainLayout.transform, "Fragmento Dormido", 26, TextAlignmentOptions.Center);
    TMP_Text loreText = CreateText("LoreText", mainLayout.transform, "Un cristal apagado aparece en una mesa olvidada.", 18, TextAlignmentOptions.Center);
    loreText.color = new Color(0.85f, 0.9f, 0.96f);
        TMP_Text coinsText = CreateText("CoinsText", mainLayout.transform, "0", 44, TextAlignmentOptions.Center);
        TMP_Text cpsText = CreateText("CoinsPerSecondText", mainLayout.transform, "0/s", 22, TextAlignmentOptions.Center);
        TMP_Text clickPowerText = CreateText("ClickPowerText", mainLayout.transform, "+1 por toque", 20, TextAlignmentOptions.Center);
        Button crystalButton = CreateButton("CrystalButton", mainLayout.transform, "CRISTAL", new Color(0.1f, 0.75f, 0.95f));
    crystalButton.gameObject.AddComponent<CrystalPulseController>();
        TMP_Text statusText = CreateText("StatusText", mainLayout.transform, "Toca el cristal para comenzar", 18, TextAlignmentOptions.Center);

        GameObject actionRow = CreateHorizontalGroup("ActionRow", mainLayout.transform);
        Button dailyRewardButton = CreateButton("DailyRewardButton", actionRow.transform, "Diaria", new Color(0.95f, 0.72f, 0.18f));
        Button rewardedAdButton = CreateButton("RewardedAdButton", actionRow.transform, "Anuncio", new Color(0.55f, 0.86f, 0.35f));

        GameObject listsRow = CreateHorizontalGroup("ListsRow", mainLayout.transform);
        RectTransform listsRowRect = listsRow.GetComponent<RectTransform>();
        listsRowRect.sizeDelta = new Vector2(0, 520);

        GameObject shopPanel = CreatePanel("ShopPanel", listsRow.transform, "MEJORAS");
        Transform shopContent = CreateScrollContent(shopPanel.transform, "ShopScrollView");

        GameObject missionsPanel = CreatePanel("MissionsPanel", listsRow.transform, "MISIONES");
        Transform missionsContent = CreateScrollContent(missionsPanel.transform, "MissionsScrollView");

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

        EditorSceneManager.SaveScene(scene, ScenePath);
        Selection.activeObject = canvas.gameObject;
        Debug.Log($"Prototype scene created at {ScenePath}");
    }

    private static void EnsureDirectories()
    {
        Directory.CreateDirectory("Assets/Scenes");
        Directory.CreateDirectory("Assets/Prefabs/UI");
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
        scaler.matchWidthOrHeight = 0.5f;

        canvasObject.AddComponent<GraphicRaycaster>();
        return canvas;
    }

    private static Image CreateBackground(Transform parent)
    {
        GameObject backgroundObject = new GameObject("DynamicBackground");
        backgroundObject.transform.SetParent(parent, false);
        Image image = backgroundObject.AddComponent<Image>();
        image.color = new Color(0.06f, 0.09f, 0.15f, 1f);

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
        image.color = new Color(0.15f, 0.86f, 1f, 0.18f);
        bandObject.AddComponent<AmbientPulseController>();

        RectTransform rect = bandObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0.42f);
        rect.anchorMax = new Vector2(1, 0.62f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        return image;
    }

    private static GameObject CreateMainLayout(Transform parent)
    {
        GameObject layoutObject = new GameObject("MainLayout");
        layoutObject.transform.SetParent(parent, false);

        RectTransform rect = layoutObject.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = new Vector2(48, 48);
        rect.offsetMax = new Vector2(-48, -48);

        VerticalLayoutGroup layout = layoutObject.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 18;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlHeight = false;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = true;

        return layoutObject;
    }

    private static GameObject CreateHorizontalGroup(string name, Transform parent)
    {
        GameObject group = new GameObject(name);
        group.transform.SetParent(parent, false);
        RectTransform rect = group.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(0, 96);

        HorizontalLayoutGroup layout = group.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 16;
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = true;
        layout.childForceExpandWidth = true;

        return group;
    }

    private static GameObject CreatePanel(string name, Transform parent, string title)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);
        Image image = panel.AddComponent<Image>();
        image.color = new Color(0.07f, 0.09f, 0.14f, 0.92f);

        VerticalLayoutGroup layout = panel.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(16, 16, 16, 16);
        layout.spacing = 12;
        layout.childControlWidth = true;
        layout.childForceExpandWidth = true;
        layout.childControlHeight = false;
        layout.childForceExpandHeight = false;

        CreateText(title + "Title", panel.transform, title, 22, TextAlignmentOptions.Center);
        return panel;
    }

    private static Transform CreateScrollContent(Transform parent, string name)
    {
        GameObject scrollObject = new GameObject(name);
        scrollObject.transform.SetParent(parent, false);
        RectTransform scrollRectTransform = scrollObject.AddComponent<RectTransform>();
        scrollRectTransform.sizeDelta = new Vector2(0, 420);

        Image background = scrollObject.AddComponent<Image>();
        background.color = new Color(0.04f, 0.05f, 0.08f, 0.75f);

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
        contentLayout.spacing = 10;
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

    private static TMP_Text CreateText(string name, Transform parent, string text, int size, TextAlignmentOptions alignment)
    {
        GameObject textObject = new GameObject(name);
        textObject.transform.SetParent(parent, false);
        TextMeshProUGUI tmp = textObject.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.alignment = alignment;
        tmp.color = Color.white;

        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(0, size + 20);
        return tmp;
    }

    private static Button CreateButton(string name, Transform parent, string label, Color color)
    {
        GameObject buttonObject = new GameObject(name);
        buttonObject.transform.SetParent(parent, false);
        Image image = buttonObject.AddComponent<Image>();
        image.color = color;
        Button button = buttonObject.AddComponent<Button>();
        button.targetGraphic = image;

        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(0, 110);

        TMP_Text text = CreateText("Label", buttonObject.transform, label, 24, TextAlignmentOptions.Center);
        RectTransform textRect = text.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        return button;
    }

    private static UpgradeButtonView CreateUpgradeButtonPrefab()
    {
        GameObject root = CreateRowBase("UpgradeButtonView");
        UpgradeButtonView view = root.AddComponent<UpgradeButtonView>();
        TMP_Text nameText = CreateText("NameText", root.transform, "Upgrade", 18, TextAlignmentOptions.Left);
        TMP_Text descriptionText = CreateText("DescriptionText", root.transform, "Description", 14, TextAlignmentOptions.Left);
        TMP_Text levelText = CreateText("LevelText", root.transform, "Nivel 0", 14, TextAlignmentOptions.Left);
        TMP_Text costText = CreateText("CostText", root.transform, "0", 16, TextAlignmentOptions.Left);
        Button buyButton = CreateButton("BuyButton", root.transform, "Comprar", new Color(0.1f, 0.65f, 0.9f));

        SetObjectReference(view, "nameText", nameText);
        SetObjectReference(view, "descriptionText", descriptionText);
        SetObjectReference(view, "levelText", levelText);
        SetObjectReference(view, "costText", costText);
        SetObjectReference(view, "buyButton", buyButton);

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, UpgradeButtonPrefabPath);
        Object.DestroyImmediate(root);
        return prefab.GetComponent<UpgradeButtonView>();
    }

    private static MissionRowView CreateMissionRowPrefab()
    {
        GameObject root = CreateRowBase("MissionRowView");
        MissionRowView view = root.AddComponent<MissionRowView>();
        TMP_Text titleText = CreateText("TitleText", root.transform, "Mission", 18, TextAlignmentOptions.Left);
        TMP_Text descriptionText = CreateText("DescriptionText", root.transform, "Description", 14, TextAlignmentOptions.Left);
        TMP_Text progressText = CreateText("ProgressText", root.transform, "0 / 0", 14, TextAlignmentOptions.Left);
        TMP_Text rewardText = CreateText("RewardText", root.transform, "+0", 16, TextAlignmentOptions.Left);
        Button claimButton = CreateButton("ClaimButton", root.transform, "Reclamar", new Color(0.95f, 0.72f, 0.18f));

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
        text.fontSize = 28;
        text.alignment = TextAlignmentOptions.Center;
        text.color = new Color(1f, 0.91f, 0.42f, 1f);

        RectTransform rect = root.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(260, 60);

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, FloatingTextPrefabPath);
        Object.DestroyImmediate(root);
        return prefab.GetComponent<TMP_Text>();
    }

    private static GameObject CreateRowBase(string name)
    {
        GameObject root = new GameObject(name);
        Image image = root.AddComponent<Image>();
        image.color = new Color(0.12f, 0.15f, 0.22f, 1f);

        VerticalLayoutGroup layout = root.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(12, 12, 12, 12);
        layout.spacing = 6;
        layout.childControlWidth = true;
        layout.childForceExpandWidth = true;
        layout.childControlHeight = false;
        layout.childForceExpandHeight = false;

        LayoutElement element = root.AddComponent<LayoutElement>();
        element.preferredHeight = 210;
        return root;
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
}
