using UnityEngine;

public class PlayerTriggerManager : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask leaveHouseLayer;

    [Header("Scripts/Collider")]
    public Collider playerCollider;
    public FamilyQuest familyQuest;

    [Header("Check Settings")]
    public float checkInterval = 0.1f; // check every 0.1 seconds
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= checkInterval)
        {
            timer = 0f;
            CheckHouseOverlap();
        }
    }

    private void CheckHouseOverlap()
    {
        // Shrink slightly so touching edges don't count
        Vector3 extents = playerCollider.bounds.extents * 0.95f;

        Collider[] hits = Physics.OverlapBox(
            playerCollider.bounds.center,
            extents,
            playerCollider.transform.rotation,
            leaveHouseLayer
        );

        bool isInside = hits.Length > 0;

        if (familyQuest.insideHouse != isInside)
        {
            familyQuest.insideHouse = isInside;
            Debug.Log("insideHouse = " + isInside);
        }
    }
}
