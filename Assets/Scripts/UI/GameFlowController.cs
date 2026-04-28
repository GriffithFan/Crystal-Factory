using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowController : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject gameplayRoot;
    [SerializeField] private GameObject mainMenuRoot;
    [SerializeField] private GameObject loadingRoot;
    [SerializeField] private GameObject storyRoot;
    [SerializeField] private GameObject storeRoot;

    [Header("Text")]
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private TMP_Text storeStatusText;

    [Header("Main Menu")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button storyButton;

    [Header("Story")]
    [SerializeField] private Button storyBackButton;

    [Header("Store")]
    [SerializeField] private Button openStoreButton;
    [SerializeField] private Button closeStoreButton;
    [SerializeField] private Button smallPackButton;
    [SerializeField] private Button mediumPackButton;
    [SerializeField] private Button removeAdsButton;

    [Header("Gameplay Tabs")]
    [SerializeField] private GameObject shopPanelRoot;
    [SerializeField] private GameObject missionsPanelRoot;
    [SerializeField] private Button shopTabButton;
    [SerializeField] private Button missionsTabButton;

    private Coroutine loadingRoutine;

    private void OnEnable()
    {
        AddListener(startButton, StartGame);
        AddListener(storyButton, ShowStory);
        AddListener(storyBackButton, ShowMainMenu);
        AddListener(openStoreButton, ShowStore);
        AddListener(closeStoreButton, HideStore);
        AddListener(smallPackButton, BuySmallPack);
        AddListener(mediumPackButton, BuyMediumPack);
        AddListener(removeAdsButton, BuyRemoveAds);
        AddListener(shopTabButton, ShowShopTab);
        AddListener(missionsTabButton, ShowMissionsTab);
    }

    private void Start()
    {
        ShowMainMenu();
    }

    private void OnDisable()
    {
        RemoveListener(startButton, StartGame);
        RemoveListener(storyButton, ShowStory);
        RemoveListener(storyBackButton, ShowMainMenu);
        RemoveListener(openStoreButton, ShowStore);
        RemoveListener(closeStoreButton, HideStore);
        RemoveListener(smallPackButton, BuySmallPack);
        RemoveListener(mediumPackButton, BuyMediumPack);
        RemoveListener(removeAdsButton, BuyRemoveAds);
        RemoveListener(shopTabButton, ShowShopTab);
        RemoveListener(missionsTabButton, ShowMissionsTab);
    }

    private void ShowMainMenu()
    {
        SetActive(gameplayRoot, false);
        SetActive(mainMenuRoot, true);
        SetActive(loadingRoot, false);
        SetActive(storyRoot, false);
        SetActive(storeRoot, false);
    }

    private void StartGame()
    {
        if (loadingRoutine != null)
        {
            StopCoroutine(loadingRoutine);
        }

        loadingRoutine = StartCoroutine(LoadIntoGame());
    }

    private IEnumerator LoadIntoGame()
    {
        SetActive(mainMenuRoot, false);
        SetActive(storyRoot, false);
        SetActive(storeRoot, false);
        SetActive(gameplayRoot, false);
        SetActive(loadingRoot, true);

        if (loadingText != null)
        {
            loadingText.text = "Calibrando nucleo de cristal...";
        }

        yield return new WaitForSeconds(0.45f);

        if (loadingText != null)
        {
            loadingText.text = "Reactivando lineas de produccion...";
        }

        yield return new WaitForSeconds(0.45f);

        if (loadingText != null)
        {
            loadingText.text = "Estacion Aurora en linea.";
        }

        yield return new WaitForSeconds(0.25f);

        SetActive(loadingRoot, false);
        SetActive(gameplayRoot, true);
        ShowShopTab();
        loadingRoutine = null;
    }

    private void ShowShopTab()
    {
        SetActive(shopPanelRoot, true);
        SetActive(missionsPanelRoot, false);
    }

    private void ShowMissionsTab()
    {
        SetActive(shopPanelRoot, false);
        SetActive(missionsPanelRoot, true);
    }

    private void ShowStory()
    {
        SetActive(mainMenuRoot, false);
        SetActive(storyRoot, true);
        SetActive(loadingRoot, false);
        SetActive(storeRoot, false);
        SetActive(gameplayRoot, false);
    }

    private void ShowStore()
    {
        SetActive(storeRoot, true);
        SetStoreStatus("Compras simuladas para balancear monetizacion sin bloquear progreso.");
    }

    private void HideStore()
    {
        SetActive(storeRoot, false);
    }

    private void BuySmallPack()
    {
        if (GameManager.Instance == null)
        {
            SetStoreStatus("La fabrica aun no esta lista.");
            return;
        }

        GameManager.Instance.IAP.BuySmallCoinPack();
        SetStoreStatus("Pack pequeno agregado: +1K cristales.");
    }

    private void BuyMediumPack()
    {
        if (GameManager.Instance == null)
        {
            SetStoreStatus("La fabrica aun no esta lista.");
            return;
        }

        GameManager.Instance.IAP.BuyMediumCoinPack();
        SetStoreStatus("Pack mediano agregado: +6K cristales.");
    }

    private void BuyRemoveAds()
    {
        if (GameManager.Instance == null)
        {
            SetStoreStatus("La fabrica aun no esta lista.");
            return;
        }

        GameManager.Instance.IAP.BuyRemoveAds();
        SetStoreStatus("Remove Ads activado en simulacion.");
    }

    private void SetStoreStatus(string message)
    {
        if (storeStatusText != null)
        {
            storeStatusText.text = message;
        }
    }

    private static void SetActive(GameObject target, bool active)
    {
        if (target != null)
        {
            target.SetActive(active);
        }
    }

    private static void AddListener(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button != null)
        {
            button.onClick.AddListener(action);
        }
    }

    private static void RemoveListener(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button != null)
        {
            button.onClick.RemoveListener(action);
        }
    }
}
