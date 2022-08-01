 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CrookedRoad : MonoBehaviour
{
    // Образующие точки кривой Безье
    public Transform p0;
    public Transform p1;
    public Transform p2;
    // Количество фрагментов дороги (Детализация)
    public int details;
    // Ширина дороги
    public float roadWidth;
    // Координаты точек Безье
    private List<Vector3> bezierPoints;
    // Вершины дороги
    private List<Vector3> vertexRoad;
    // Временная хуйня
    public GameObject gm;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        bezierPoints = new List<Vector3>();
        vertexRoad = new List<Vector3>();
        // Создаем массив из формирующих точек кривой безье
        DrawQuadraticBezierCurve(p0.position, p1.position, p2.position);

        GetEndpoints(bezierPoints[0], bezierPoints[1]);
        GetEndpoints(bezierPoints[bezierPoints.Count - 1], bezierPoints[bezierPoints.Count - 2]);

        for (int i = 1; i < details; i++)
            GetBendOfRoad(bezierPoints[i - 1], bezierPoints[i], bezierPoints[i + 1]);

        //GetBendOfRoad(bezierPoints[0], bezierPoints[1], bezierPoints[2]);

        DrawLine(bezierPoints.Count, ref bezierPoints);

        CreateMesh();


    }

    // Составляет кривую Безье по трем координатам
    private void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
    {
        float t = 0f;
        Vector3 B;
        for (int i = 0; i < details; i++)
        {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            bezierPoints.Add(B);
            t += (1 / (float)details);
        }
        bezierPoints.Add(point2);
    }

    private void DrawLine(int count, ref List<Vector3> v)
    {
        lineRenderer.positionCount = count;
        for (int i = 0; i < count; i++)
            lineRenderer.SetPosition(i, v[i]);
    }

    // Записывает координаты вершин для конца дороги в лист vertexRoad
    private void GetEndpoints(Vector3 a, Vector3 b)
    {
        Vector3 delta = b - a;
        double arctgA = Math.Atan(delta.x / delta.z);
        Vector3 offset = new Vector3((float)Math.Cos(-arctgA) * roadWidth, 0f, (float)Math.Sin(-arctgA) * roadWidth);
        vertexRoad.Add(a + offset);
        vertexRoad.Add(a - offset);
    }

    // Записывает координаты вершин на месте изгиба дороги в лист vertexRoad
    private void GetBendOfRoad(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 delta = b - a;
        double arctgA = Math.Atan(delta.x / delta.z);
        Vector3 AB = b - a;
        Vector3 BC = b - c;
        double lenAB = Math.Sqrt(AB.x * AB.x + AB.z * AB.z);
        double lenBC = Math.Sqrt(BC.x * BC.x + BC.z * BC.z);
        // Арккосинус угла p1p2p3 деленный на два
        double arccos = Math.Acos((AB.x * BC.x + AB.z * BC.z) / (lenAB * lenBC)) / 2;
        Debug.Log(arccos / Math.PI * 180);
        Vector3 offset = new Vector3((float)Math.Cos(arccos - arctgA + Math.PI / 2) * roadWidth, 0f, (float)Math.Sin(arccos - arctgA + Math.PI/2) * roadWidth);
        vertexRoad.Add(b + offset);
        vertexRoad.Add(b - offset);
    }

    private void CreateMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        Vector3[] V = new Vector3[3];
        V[0] = vertexRoad[0];
        V[1] = vertexRoad[1];
        V[2] = vertexRoad[2];

        mesh.vertices = V;

        int[] triangles = new int[] { 0, 1, 2 };
        mesh.triangles = triangles;
        
        Debug.Log(mesh.vertices[1]);
        Debug.Log(mesh.vertices[2]);
        Debug.Log(mesh.vertices[0]);
        /*
        int[] triangles = new int[(vertexRoad.Count - 2) * 3];
        for (int i = 0; i < vertexRoad.Count - 2; i++)
        {
            triangles[i * 3] = i;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }
        mesh.triangles = triangles;
        */
    }
}
