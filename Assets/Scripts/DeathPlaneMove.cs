using UnityEngine;
using System.Collections;

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

    void Start()
    {
        if (objectToActivate != null)
            objectToActivate.SetActive(false);

        StartCoroutine(ActivateAfterDelay());
    }

    private IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(activateAfterSeconds);

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
            Application.Quit();
        }
    }
}
