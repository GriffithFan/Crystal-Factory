using UnityEngine;
using UnityEngine.UI;

public class AmbientPulseController : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private float speed = 1.4f;
    [SerializeField] private float alphaMin = 0.12f;
    [SerializeField] private float alphaMax = 0.28f;

    private Color baseColor;

    private void Awake()
    {
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }

        if (targetImage != null)
        {
            baseColor = targetImage.color;
        }
    }

    private void Update()
    {
        if (targetImage == null)
        {
            return;
        }

        float alpha = Mathf.Lerp(alphaMin, alphaMax, (Mathf.Sin(Time.time * speed) + 1) * 0.5f);
        targetImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
    }
}
