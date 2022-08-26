using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSegment : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    
    public Vector3 CalculateMidPoint(Vector3 v1, Vector3 v2)
    {
        return v1 + (v2 - v1) / 2;
    }

    private void FixedUpdate()
    {
        transform.position = CalculateMidPoint(startPoint, endPoint);
        Vector3 scale = transform.localScale;
        scale.z = Vector3.Distance(startPoint, endPoint);
        transform.localScale = scale;
        transform.LookAt(endPoint);
    }
}
