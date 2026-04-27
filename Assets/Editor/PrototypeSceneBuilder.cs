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
        CreateAmbientGlow("TopAmbientGlow", canvas.transform, new Vector2(-0.18f, 0.7f), new Vector2(0.82f, 1.16f), new Color(0.1f, 0.7f, 1f, 0.18f));
        CreateAmbientGlow("BottomAmbientGlow", canvas.transform, new Vector2(0.12f, -0.12f), new Vector2(1.18f, 0.36f), new Color(0.45f, 0.14f, 0.9f, 0.12f));
        Image energyBandImage = CreateEnergyBand(canvas.transform);
        GameObject phoneRoot = CreatePhoneRoot(canvas.transform);
        GameObject mainLayout = CreateMainLayout(phoneRoot.transform);

        TMP_Text titleText = CreateText("GameTitleText", mainLayout.transform, "CRYSTAL FACTORY", 54, 64, TextAlignmentOptions.Center);
        titleText.color = new Color(0.72f, 0.96f, 1f);
        TMP_Text stageTitleText = CreateText("StageTitleText", mainLayout.transform, "Fragmento Dormido", 30, 38, TextAlignmentOptions.Center);
        TMP_Text loreText = CreateText("LoreText", mainLayout.transform, "Un cristal apagado aparece en una mesa olvidada. Cada toque despierta una vibracion antigua bajo el laboratorio.", 22, 62, TextAlignmentOptions.Center);
        loreText.color = new Color(0.85f, 0.9f, 0.96f);
        TMP_Text coinsText = CreateText("CoinsText", mainLayout.transform, "0", 72, 76, TextAlignmentOptions.Center);
        TMP_Text cpsText = CreateText("CoinsPerSecondText", mainLayout.transform, "0/s", 26, 30, TextAlignmentOptions.Center);
        TMP_Text clickPowerText = CreateText("ClickPowerText", mainLayout.transform, "+1 por toque", 21, 28, TextAlignmentOptions.Center);

        Button crystalButton = CreateCrystalButton(mainLayout.transform);
        TMP_Text statusText = CreateText("StatusText", mainLayout.transform, "Toca el cristal para comenzar", 20, 40, TextAlignmentOptions.Center);
        statusText.color = new Color(0.72f, 0.83f, 0.9f);

        GameObject actionRow = CreateHorizontalGroup("ActionRow", mainLayout.transform, 82);
        Button dailyRewardButton = CreateButton("DailyRewardButton", actionRow.transform, "Diaria", new Color(0.35f, 0.24f, 0.07f), 24, 82);
        Button rewardedAdButton = CreateButton("RewardedAdButton", actionRow.transform, "Boost", new Color(0.08f, 0.28f, 0.2f), 24, 82);

        GameObject listsColumn = CreateVerticalGroup("ListsColumn", mainLayout.transform, 770, 12);
        GameObject shopPanel = CreatePanel("ShopPanel", listsColumn.transform, "MEJORAS", 430);
        Transform shopContent = CreateScrollContent(shopPanel.transform, "ShopScrollView", 356);

        GameObject missionsPanel = CreatePanel("MissionsPanel", listsColumn.transform, "MISIONES", 328);
        Transform missionsContent = CreateScrollContent(missionsPanel.transform, "MissionsScrollView", 254);

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
        image.color = new Color(0.15f, 0.86f, 1f, 0.16f);
        bandObject.AddComponent<AmbientPulseController>();

        RectTransform rect = bandObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0.47f);
        rect.anchorMax = new Vector2(1, 0.58f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        return image;
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
        layout.spacing = 9;
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
        rect.sizeDelta = new Vector2(0, 440);
        AddLayoutElement(buttonObject, 440);

        Button button = buttonObject.AddComponent<Button>();
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
        RectTransform pedestalRect = pedestalObject.GetComponent<RectTransform>();
        pedestalRect.anchorMin = new Vector2(0.3f, 0.04f);
        pedestalRect.anchorMax = new Vector2(0.7f, 0.14f);
        pedestalRect.offsetMin = Vector2.zero;
        pedestalRect.offsetMax = Vector2.zero;

        GameObject crystalObject = new GameObject("CrystalGem");
        crystalObject.transform.SetParent(buttonObject.transform, false);
        CrystalGraphic crystal = crystalObject.AddComponent<CrystalGraphic>();
        RectTransform crystalRect = crystalObject.GetComponent<RectTransform>();
        crystalRect.anchorMin = new Vector2(0.28f, 0.12f);
        crystalRect.anchorMax = new Vector2(0.72f, 0.9f);
        crystalRect.offsetMin = Vector2.zero;
        crystalRect.offsetMax = Vector2.zero;

        button.targetGraphic = crystal;
        SetObjectReference(pulse, "target", crystalObject.transform);
        return button;
    }

    private static GameObject CreateEllipse(string name, Transform parent, Color centerColor, Color edgeColor)
    {
        GameObject ellipseObject = new GameObject(name);
        ellipseObject.transform.SetParent(parent, false);
        SoftEllipseGraphic ellipse = ellipseObject.AddComponent<SoftEllipseGraphic>();
        SetColorReference(ellipse, "centerColor", centerColor);
        SetColorReference(ellipse, "edgeColor", edgeColor);
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
        RectTransform textRect = text.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        return button;
    }

    private static UpgradeButtonView CreateUpgradeButtonPrefab()
    {
        GameObject root = CreateRowBase("UpgradeButtonView", 178);
        UpgradeButtonView view = root.AddComponent<UpgradeButtonView>();
        TMP_Text nameText = CreateText("NameText", root.transform, "Upgrade", 18, 28, TextAlignmentOptions.Left);
        TMP_Text descriptionText = CreateText("DescriptionText", root.transform, "Description", 14, 40, TextAlignmentOptions.Left);
        TMP_Text levelText = CreateText("LevelText", root.transform, "Nivel 0", 14, 24, TextAlignmentOptions.Left);
        TMP_Text costText = CreateText("CostText", root.transform, "0", 16, 26, TextAlignmentOptions.Left);
        Button buyButton = CreateButton("BuyButton", root.transform, "Comprar", new Color(0.1f, 0.65f, 0.9f), 16, 42);

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
        GameObject root = CreateRowBase("MissionRowView", 172);
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

    private static GameObject CreateRowBase(string name, float height)
    {
        GameObject root = new GameObject(name);
        Image image = root.AddComponent<Image>();
        image.color = new Color(0.12f, 0.15f, 0.22f, 1f);

        VerticalLayoutGroup layout = root.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(12, 12, 10, 10);
        layout.spacing = 4;
        layout.childControlWidth = true;
        layout.childForceExpandWidth = true;
        layout.childControlHeight = false;
        layout.childForceExpandHeight = false;

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
