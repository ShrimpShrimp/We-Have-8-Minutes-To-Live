using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathPlaneMove : MonoBehaviour
{
    [Header("Activation Settings")]
    public float activateAfterSeconds = 3f;
    public GameObject objectToActivate;
    public float remainingTime;

    [Header("Spawn Settings")]
    public Transform player;              // Assign player here
    public Vector3 offsetFromPlayer;      // Position relative to player (local)

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private bool canMove = false;

    [Header("Visual FX")]
    public Renderer objectRenderer;  // Assign the object's renderer
    public float fadeDuration = 2f;  // Seconds to fade in
    public bool scrollTexture = true;
    public float scrollSpeedX = 0.1f;
    public float scrollSpeedY = 0f;

    private Material objectMaterial;
    private bool fadeComplete = false;


    [Header("UI Object")]
    public TextMeshProUGUI timerText;
    public Animator animator;
    public int countdownAlarmTime = 10;
    private bool animationStarted = false;

    void Start()
    {
        if (objectToActivate != null)
            objectToActivate.SetActive(false);

        if (timerText != null){
            timerText.gameObject.SetActive(true);

            int totalSeconds = Mathf.CeilToInt(activateAfterSeconds);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        StartCoroutine(ActivateAfterDelay());
    }

    private IEnumerator ActivateAfterDelay()
    {
        remainingTime = activateAfterSeconds;

        // countdown loop
        while (remainingTime > 0f)
        {
            if (timerText != null)
            {
                int totalSeconds = Mathf.CeilToInt(remainingTime);
                int minutes = totalSeconds / 60;
                int seconds = totalSeconds % 60;

                timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

                if (!animationStarted && totalSeconds <= countdownAlarmTime){
                    animator.SetTrigger("Countdown Alarm");
                    animationStarted = true;
                }
            }



            remainingTime -= Time.deltaTime;
            yield return null;
        }

        // hide UI when finished
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }

        // Spawn at player's position + offset
        if (player != null && objectToActivate != null)
        {
            Vector3 worldSpawnPosition = player.position + offsetFromPlayer;
            objectToActivate.transform.position = worldSpawnPosition;
            objectToActivate.SetActive(true);

            // prepare fading & scrolling
            if (objectRenderer != null)
            {
                objectMaterial = objectRenderer.material;

                // Start fully transparent
                Color c = objectMaterial.color;
                c.a = 0f;
                objectMaterial.color = c;

                StartCoroutine(FadeInMaterial());
            }

        }

        canMove = true;
    }

    void Update()
    {
        if (canMove && objectToActivate != null)
        {
            objectToActivate.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }

        if (scrollTexture && objectMaterial != null)
        {
            Vector2 offset = objectMaterial.mainTextureOffset;
            offset.x += scrollSpeedX * Time.deltaTime;
            offset.y += scrollSpeedY * Time.deltaTime;
            objectMaterial.mainTextureOffset = offset;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathPlane"))
        {
            Debug.Log("Game should end here.");
            SceneManager.LoadScene("EndMenu");
        }
    }

    private IEnumerator FadeInMaterial()
    {
        float t = 0f;
        fadeComplete = false;

        // Get current color (including alpha)
        Color start = objectMaterial.GetColor("_Color");
        Color end = start;
        end.a = 1f;

        // Start from alpha 0
        start.a = 0f;
        objectMaterial.SetColor("_Color", start);

        while (t < fadeDuration)
        {
            float progress = t / fadeDuration;
            Color lerped = Color.Lerp(start, end, progress);
            objectMaterial.SetColor("_Color", lerped);

            t += Time.deltaTime;
            yield return null;
        }

        objectMaterial.SetColor("_Color", end);
        fadeComplete = true;
    }
}
