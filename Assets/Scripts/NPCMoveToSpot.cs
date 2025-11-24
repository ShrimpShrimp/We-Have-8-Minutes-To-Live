using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMoveToSpot : MonoBehaviour
{
    // Stores queues of positions for each NPC transform
    private static Dictionary<Transform, Queue<Vector3>> moveQueues = new Dictionary<Transform, Queue<Vector3>>();

    // Tracks which NPCs are currently moving
    private static HashSet<Transform> movingNPCs = new HashSet<Transform>();

    // Call this to queue a move for any NPC transform at given speed
    public static void MoveToPosition(MonoBehaviour coroutineRunner, Transform npcTransform, Vector3 targetPosition, float speed)
    {
        if (!moveQueues.ContainsKey(npcTransform))
        {
            moveQueues[npcTransform] = new Queue<Vector3>();
        }

        moveQueues[npcTransform].Enqueue(targetPosition);

        if (!movingNPCs.Contains(npcTransform))
        {
            coroutineRunner.StartCoroutine(ProcessQueue(coroutineRunner, npcTransform, speed));
        }
    }

    // Coroutine that processes the queued moves sequentially
    private static IEnumerator ProcessQueue(MonoBehaviour coroutineRunner, Transform npcTransform, float speed)
    {
        movingNPCs.Add(npcTransform);

        Collider npcCollider = npcTransform.GetComponent<Collider>();

        while (moveQueues[npcTransform].Count > 0)
        {
            Vector3 targetPos = moveQueues[npcTransform].Dequeue();

            if (npcCollider != null)
                npcCollider.enabled = false;

            while (Vector3.Distance(npcTransform.position, targetPos) > 0.01f)
            {
                npcTransform.position = Vector3.MoveTowards(npcTransform.position, targetPos, speed * Time.deltaTime);
                yield return null;
            }

            npcTransform.position = targetPos;

            if (npcCollider != null)
                npcCollider.enabled = true;

            // small yield to let other coroutines run before next move
            yield return null;
        }

        movingNPCs.Remove(npcTransform);
    }
}
