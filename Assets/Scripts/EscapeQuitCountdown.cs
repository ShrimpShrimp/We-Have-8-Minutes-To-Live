using UnityEngine;
using TMPro;

public class EscapeQuitCountdown : MonoBehaviour
{
    public float holdDuration = 3f;           // Seconds to hold Escape to quit
    public TextMeshProUGUI countdownText;     // Assign your TMP text in Inspector

    private float holdTimer = 0f;
    private bool isCountingDown = false;

    void Start()
    {
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (!isCountingDown)
            {
                isCountingDown = true;
                holdTimer = holdDuration;
                if (countdownText != null)
                    countdownText.gameObject.SetActive(true);
            }

            holdTimer -= Time.deltaTime;

            if (countdownText != null)
                countdownText.text = $"Quitting in {Mathf.Ceil(holdTimer)}...";

            if (holdTimer <= 0f)
            {
                QuitApplication();
            }
        }
        else
        {
            if (isCountingDown)
            {
                isCountingDown = false;
                if (countdownText != null)
                    countdownText.gameObject.SetActive(false);
            }
        }
    }

    void QuitApplication()
    {
        Debug.Log("Quitting application...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in Editor
#else
        Application.Quit(); // Quit built app
#endif
    }
}
