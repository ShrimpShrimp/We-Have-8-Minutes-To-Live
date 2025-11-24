using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathPlaneMove : MonoBehaviour
{
    [Header("Activation Settings")]
    public float activateAfterSeconds = 3f;
    public GameObject objectToActivate;

    [Header("Spawn Settings")]
    public Transform player;              // Assign player here
    public Vector3 offsetFromPlayer;      // Position relative to player (local)

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private bool canMove = false;


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
        float remaining = activateAfterSeconds;

        // countdown loop
        while (remaining > 0f)
        {
            if (timerText != null)
            {
                int totalSeconds = Mathf.CeilToInt(remaining);
                int minutes = totalSeconds / 60;
                int seconds = totalSeconds % 60;

                timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

                if (!animationStarted && totalSeconds <= countdownAlarmTime){
                    animator.SetTrigger("Countdown Alarm");
                    animationStarted = true;
                }
            }



            remaining -= Time.deltaTime;
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
        }

        canMove = true;
    }

    void Update()
    {
        if (canMove && objectToActivate != null)
        {
            objectToActivate.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
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
}
