using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageDisplayController : MonoBehaviour
{
    [SerializeField] private TMP_Text stageTitleText;
    [SerializeField] private TMP_Text loreText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image accentImage;

    private void OnEnable()
    {
        GameEvents.StageChanged += UpdateStage;
    }

    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.Stages.CurrentStage != null)
        {
            UpdateStage(GameManager.Instance.Stages.CurrentStage);
        }
    }

    private void OnDisable()
    {
        GameEvents.StageChanged -= UpdateStage;
    }

    private void UpdateStage(StageDefinition stage)
    {
        if (stage == null)
        {
            return;
        }

        if (stageTitleText != null)
        {
            stageTitleText.text = stage.displayName;
        }

        if (loreText != null)
        {
            loreText.text = stage.lore;
        }

        if (backgroundImage != null && ColorUtility.TryParseHtmlString(stage.primaryColor, out Color primaryColor))
        {
            Sprite backgroundSprite = string.IsNullOrEmpty(stage.backgroundResource) ? null : Resources.Load<Sprite>(stage.backgroundResource);
            backgroundImage.sprite = backgroundSprite;
            backgroundImage.color = backgroundSprite == null ? primaryColor : Color.white;
            backgroundImage.preserveAspect = false;
        }

        if (accentImage != null && ColorUtility.TryParseHtmlString(stage.accentColor, out Color accentColor))
        {
            accentImage.color = accentColor;
        }
    }
}
