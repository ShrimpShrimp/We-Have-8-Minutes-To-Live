using UnityEngine;
using System.Collections;

public class FadePanel : MonoBehaviour
{
    [Header("Target UI Panel (must have CanvasGroup)")]
    public CanvasGroup panel;

    [Header("Fade Settings")]
    public float fadeDuration = 0.5f;

    private Coroutine fadeRoutine;

    private void Awake()
    {
        // If the panel is disabled in the hierarchy, accessing CanvasGroup is still fine.
        // But we must NOT try to change alpha until we activate it.
        if (panel == null)
            panel = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        FadeOut();
    }

    // -----------------------------
    // PUBLIC METHODS (you call these)
    // -----------------------------

    public void FadeIn()
    {
        // Activate panel BEFORE fading
        panel.gameObject.SetActive(true);

        // Ensure only one fade runs at a time
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeCanvasGroup(panel, panel.alpha, 1f, false));
    }

    public void FadeOut()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeCanvasGroup(panel, panel.alpha, 0f, true));
    }

    // -----------------------------
    // INTERNAL FADE COROUTINE
    // -----------------------------

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, bool disableOnEnd)
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, time / fadeDuration);
            yield return null;
        }

        cg.alpha = end;

        // Turn panel off AFTER it is fully faded out
        if (disableOnEnd)
            cg.gameObject.SetActive(false);

        fadeRoutine = null;
    }
}
