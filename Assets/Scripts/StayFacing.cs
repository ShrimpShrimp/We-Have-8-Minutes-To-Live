using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayFacing : MonoBehaviour
{
    public Camera cam;

    void Update()
    {
        if (cam != null)
        {
            Vector3 dir = transform.position - cam.transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
