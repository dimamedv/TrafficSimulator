using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadEditor : MonoBehaviour
{
    public LayerMask layerMask;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(RayFromCursor.ray, out hit, 1000, layerMask))
            {
                Transform objectHit = hit.transform;
                Debug.Log(objectHit.name);

                //objectHit.childCount;
            }
        }
    }
}
