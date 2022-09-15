using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadEditor : MonoBehaviour
{
    public LayerMask layerMask;


    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(RayFromCursor.ray, out hit, 1000, layerMask))
        {
            Debug.Log(hit.transform.name);
        }
    }
}
