using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineVisualization : AbstractVisualization
{
    public override void RenderingRoad()
    {
        Vector3[] ArrayOfPoints = new Vector3[abstractRoad.points.Count];
        for (int i = 0; i < abstractRoad.points.Count; i++)
            ArrayOfPoints[i] = abstractRoad.points[i];
        LineRenderer line = gameObject.GetComponent<LineRenderer>();
        line.endColor = Color.blue;
        line.startColor = Color.red;
        line.positionCount = abstractRoad.points.Count;
        line.SetPositions(ArrayOfPoints);
        line.enabled = true;
    }
}
