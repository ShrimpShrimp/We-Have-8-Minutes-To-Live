using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageStretchSpawner : MonoBehaviour
{
    [Header("Source and Parent")]
    [Tooltip("The original UI Image in the Canvas that stays visible and is used as a template.")]
    public RectTransform sourceImage;
    [Tooltip("Parent RectTransform to stretch to. Leave empty to use the source image's parent.")]
    public RectTransform parent;

    [Header("Spawning")]
    [Tooltip("How often a new clone is spawned (seconds).")]
    public float spawnInterval = 0.3f;

    [Header("Timing")]
    [Tooltip("How long it takes for the bar to stretch from original width to full width.")]
    public float stretchTime = 0.4f;
    [Tooltip("How long before the clone is fully faded and destroyed.")]
    public float lifeTime = 0.8f;

    [Header("Appearance")]
    [Range(0f, 1f)]
    [Tooltip("Starting opacity of each spawned bar.")]
    public float startAlpha = 1f;
    [Tooltip("Extra width beyond the parent's borders. 0 means exactly edge to edge.")]
    public float overshoot = 0f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnClone();
        }
    }

    void SpawnClone()
    {
        if (sourceImage == null)
        {
            Debug.LogWarning("ImageStretchSpawner: SourceImage is not assigned.");
            return;
        }

        RectTransform parentToUse = parent != null
            ? parent
            : sourceImage.parent as RectTransform;

        if (parentToUse == null)
        {
            Debug.LogWarning("ImageStretchSpawner: No valid parent RectTransform found.");
            return;
        }

        // Instantiate a clone as child of the parent
        RectTransform clone = Instantiate(sourceImage, parentToUse);

        // Keep it visually on top
        clone.SetAsLastSibling();

        // Match transform of the source
        clone.anchoredPosition = sourceImage.anchoredPosition;
        clone.localRotation    = sourceImage.localRotation;
        clone.localScale       = sourceImage.localScale;

        Image cloneImage = clone.GetComponent<Image>();
        if (cloneImage == null)
        {
            Debug.LogWarning("ImageStretchSpawner: SourceImage has no Image component.");
            Destroy(clone.gameObject);
            return;
        }

        StartCoroutine(StretchAndFade(clone, cloneImage));
    }

    IEnumerator StretchAndFade(RectTransform rect, Image img)
    {
        if (rect == null || img == null) yield break;

        float elapsed = 0f;

        // Set initial color with custom alpha
        Color baseColor = img.color;
        baseColor.a = startAlpha;
        img.color = baseColor;

        // Size at spawn
        Vector2 originalSize = rect.sizeDelta;
        float startWidth = originalSize.x;
        float height = originalSize.y;

        // Calculate target width based on parent
        RectTransform parentRect = rect.parent as RectTransform;
        float parentWidth = parentRect != null ? parentRect.rect.width : startWidth;
        float targetWidth = parentWidth + overshoot;

        while (elapsed < lifeTime)
        {
            elapsed += Time.deltaTime;

            // Stretch progress
            float stretchT = stretchTime > 0f ? Mathf.Clamp01(elapsed / stretchTime) : 1f;
            float currentWidth = Mathf.Lerp(startWidth, targetWidth, stretchT);
            rect.sizeDelta = new Vector2(currentWidth, height);

            // Fade progress
            float fadeT = lifeTime > 0f ? Mathf.Clamp01(elapsed / lifeTime) : 1f;
            Color c = baseColor;
            c.a = Mathf.Lerp(startAlpha, 0f, fadeT);
            img.color = c;

            yield return null;
        }

        Destroy(rect.gameObject);
    }
}
