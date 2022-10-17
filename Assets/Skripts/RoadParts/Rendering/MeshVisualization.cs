using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MeshVisualization : AbstractVisualization
{
    private List<Vector3> _vertexRoad = new List<Vector3>(); // Вершины излома дороги (для полигонов)

    public override void RenderingRoad(List<Vector3> points)
    {
        _vertexRoad.Clear();
        CalculateRoadVertexes(points);
        CreateMesh();
    }

    // Рассчитывает координаты точек излома дороги
    private void CalculateRoadVertexes(List<Vector3> points)
    {
        Vector3 lineDirection;
        for (int pointIndex = 0; pointIndex < points.Count - 1; pointIndex++)
        {
            lineDirection = MyMath.CalculateLineDirection(points[pointIndex + 1], points[pointIndex]);
            AddMeshVertexes(points[pointIndex], lineDirection);
        }

        lineDirection = MyMath.CalculateLineDirection(points[^1], points[^2]);
        AddMeshVertexes(points[^1], lineDirection);
    }



    // Добавляет координаты точек "излома" дороги
    public void AddMeshVertexes(Vector3 a, Vector3 lineDirection)
    {
        Vector3 v1 = a + Quaternion.Euler(0, -90, 0) * 
            lineDirection * GlobalSettings.width * gameObject.GetComponent<AbstractRoad>()._countLanes / 2;
        Vector3 v2 = a + Quaternion.Euler(0, +90, 0) * 
            lineDirection * GlobalSettings.width * gameObject.GetComponent<AbstractRoad>()._countLanes / 2;

        _vertexRoad.Add(v1);
        _vertexRoad.Add(v2);
    }

    // Строит меши по точкам "излома" дороги
    private void CreateMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        // Звбивает координаты вершин в меш
        Vector3[] v = new Vector3[_vertexRoad.Count];
        for (int i = 0; i < _vertexRoad.Count; i++)
            v[i] = _vertexRoad[i];
        mesh.vertices = v;

        // Индексы вершин излома из mesh.vertices
        int[] triangles = new int[(_vertexRoad.Count - 2) * 3 * 2];

        for (int i = 0; i < (_vertexRoad.Count - 2) * 2; i++)
        {
            int j = i / 2;

            for (int k = 0; k < 3; k++) triangles[i * 3 + k] = j++;
            i++;

            for (int k = 0; k < 3; k++) triangles[i * 3 + k] = --j;
        }

        mesh.triangles = triangles;

        MeshCollider mc = GetComponent<MeshCollider>();
        Vector3 StartPostPos = gameObject.GetComponent<AbstractRoad>().startPost.transform.position;
        Vector3 EndPostPos = gameObject.GetComponent<AbstractRoad>().endPost.transform.position;
        if (StartPostPos != EndPostPos)
        {
            mc.enabled = true;
            mc.sharedMesh = mesh;
        }
        else
        {
            mc.enabled = false;
        }
    }
}
