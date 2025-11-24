using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndingBarStretchSpawner : MonoBehaviour
{
    [Header("Source and Parent")]
    [Tooltip("The original UI Image in the Canvas that stays visible and is used as a template. Rotate this 90° around Z.")]
    public RectTransform sourceImage;
    [Tooltip("Parent RectTransform to stretch inside. Leave empty to use sourceImage's parent.")]
    public RectTransform parent;

    [Header("Spawning")]
    [Tooltip("How often a new clone is spawned (seconds). Set a big number or disable autoSpawn if you want manual control.")]
    public float spawnInterval = 0.3f;
    public bool autoSpawn = true;   // Turn off if you want to call SpawnOnce() manually

    [Header("Timing")]
    [Tooltip("How long it takes for the bar to reach its final stretch size.")]
    public float stretchTime = 0.6f;
    [Tooltip("How long before the clone is fully faded and destroyed.")]
    public float lifeTime = 1.0f;

    [Header("Appearance")]
    [Range(0f, 1f)]
    [Tooltip("Starting opacity of each spawned bar.")]
    public float startAlpha = 1f;

    [Tooltip("Extra length beyond the parent's horizontal width (on screen). 0 = exactly edge to edge.")]
    public float horizontalOvershoot = 0f;

    [Tooltip("Multiplier for the bar's vertical thickness. 1 = original thickness.")]
    public float verticalThicknessMultiplier = 1.1f;

    private float timer = 0f;

    void Update()
    {
        if (!autoSpawn) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnOnce();
        }
    }

    // Call this from your ending logic if you want manual pulses:
    //   endingBarStretchSpawner.SpawnOnce();
    public void SpawnOnce()
    {
        if (sourceImage == null)
        {
            Debug.LogWarning("EndingBarStretchSpawner: SourceImage is not assigned.");
            return;
        }

        RectTransform parentToUse = parent != null
            ? parent
            : sourceImage.parent as RectTransform;

        if (parentToUse == null)
        {
            Debug.LogWarning("EndingBarStretchSpawner: No valid parent RectTransform found.");
            return;
        }

        // Instantiate a clone as child of the parent
        RectTransform clone = Instantiate(sourceImage, parentToUse);

        // Keep it visually on top
        clone.SetAsLastSibling();

        // Match transform of the source (including the 90° rotation you set)
        clone.anchoredPosition = sourceImage.anchoredPosition;
        clone.localRotation    = sourceImage.localRotation;
        clone.localScale       = sourceImage.localScale;

        Image cloneImage = clone.GetComponent<Image>();
        if (cloneImage == null)
        {
            Debug.LogWarning("EndingBarStretchSpawner: SourceImage has no Image component.");
            Destroy(clone.gameObject);
            return;
        }

        StartCoroutine(StretchAndFade(clone, cloneImage));
    }

    IEnumerator StretchAndFade(RectTransform rect, Image img)
    {
        if (rect == null || img == null) yield break;

        float elapsed = 0f;

        // Get the source alpha at spawn time
        float sourceAlpha = sourceImage.GetComponent<Image>().color.a;
        float initialAlpha = startAlpha * sourceAlpha;

        // Set initial color with custom alpha
        Color baseColor = img.color;
        baseColor.a = initialAlpha;
        img.color = baseColor;

        // NOTE: Rect is rotated 90°.
        // So:
        //   rect.sizeDelta.y = visible left/right length (main)
        //   rect.sizeDelta.x = visible thickness (vertical)
        Vector2 originalSize = rect.sizeDelta;
        float startThickness = originalSize.x;  // vertical thickness
        float startLength    = originalSize.y;  // horizontal length on screen

        RectTransform parentRect = rect.parent as RectTransform;
        float parentWidth = parentRect != null ? parentRect.rect.width : startLength;

        // Main stretch: across the screen horizontally (uses parent width),
        // but because we're rotated 90°, this maps to sizeDelta.y.
        float targetLength = parentWidth + horizontalOvershoot;

        // Slight vertical stretch: make the bar a bit thicker
        float targetThickness = startThickness * verticalThicknessMultiplier;

        while (elapsed < lifeTime)
        {
            elapsed += Time.deltaTime;

            // Stretch progress
            float stretchT = stretchTime > 0f ? Mathf.Clamp01(elapsed / stretchTime) : 1f;

            float currentLength    = Mathf.Lerp(startLength,    targetLength,    stretchT);
            float currentThickness = Mathf.Lerp(startThickness, targetThickness, stretchT);

            // x = thickness (vertical on screen), y = length (horizontal on screen)
            rect.sizeDelta = new Vector2(currentThickness, currentLength);

            // Fade progress
            float fadeT = lifeTime > 0f ? Mathf.Clamp01(elapsed / lifeTime) : 1f;
            Color c = baseColor;
            c.a = Mathf.Lerp(initialAlpha, 0f, fadeT);
            img.color = c;

            yield return null;
        }

        Destroy(rect.gameObject);
    }
}
