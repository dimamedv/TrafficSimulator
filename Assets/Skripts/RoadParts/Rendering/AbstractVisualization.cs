using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractVisualization : MonoBehaviour
{
    private List<Vector3> points;

    public abstract void RenderingRoad(List<Vector3> points);
}
