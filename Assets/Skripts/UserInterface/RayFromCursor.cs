using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayFromCursor : MonoBehaviour
{
    public static Ray ray;

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction);
        
        
    }
}
