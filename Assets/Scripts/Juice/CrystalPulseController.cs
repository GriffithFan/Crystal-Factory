using System.Collections;
using UnityEngine;

public class CrystalPulseController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float pulseScale = 1.08f;
    [SerializeField] private float pulseDuration = 0.08f;
    [SerializeField] private bool vibrateEveryTenClicks;

    private Vector3 originalScale;
    private Coroutine pulseRoutine;
    private int clickCount;

    private void Awake()
    {
        if (target == null)
        {
            target = transform;
        }

        originalScale = target.localScale;
    }

    private void OnEnable()
    {
        GameEvents.MainCrystalClicked += Pulse;
    }

    private void OnDisable()
    {
        GameEvents.MainCrystalClicked -= Pulse;
    }

    private void Pulse()
    {
        clickCount++;

        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
        }

        pulseRoutine = StartCoroutine(PulseRoutine());

        if (vibrateEveryTenClicks && clickCount % 10 == 0)
        {
            Handheld.Vibrate();
        }
    }

    private IEnumerator PulseRoutine()
    {
        target.localScale = originalScale * pulseScale;
        yield return new WaitForSeconds(pulseDuration);
        target.localScale = originalScale;
    }
}
