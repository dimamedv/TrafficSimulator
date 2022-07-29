 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrookedRoad : MonoBehaviour
{
    public Transform p0;
    public Transform p1;
    public Transform p2;
    public int details;
    private List<Vector3> bezierPoints;
    private List<int> vertexRoad;
    public GameObject gm;

    // Start is called before the first frame update
    void Start()
    {
        bezierPoints = new List<Vector3>();
        vertexRoad = new List<int>();
        // Создаем массив из формирующих точек кривой безье
        DrawQuadraticBezierCurve(p0.position, p1.position, p2.position);

        for (int i = 0; i < details; i++)
        {
            Transform t = gm.transform;
            t.position = bezierPoints[i];
            Instantiate(gm, bezierPoints[i], Quaternion.identity);
        }
    }

    void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
    {
        float t = 0f;
        Vector3 B;
        for (int i = 0; i < details; i++)
        {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            bezierPoints.Add(B);
            t += (1 / (float)details);
        }
    }
}
