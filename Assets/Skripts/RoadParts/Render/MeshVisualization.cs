using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalSettings;

public class MeshVisualization : AbstractVisualization
{
    private List<Vector3> _vertexRoad = new List<Vector3>(); // Вершины излома дороги

    public override void RenderingRoad()
    {
        abstractRoad = gameObject.GetComponent<AbstractRoad>();
        _vertexRoad.Clear();
        ConnectMeshWithParent();
        CalculateMeshVertexPoints();
        CreateMesh();
    }

    private void ConnectMeshWithParent()
    {
        if (abstractRoad.parentConnection && abstractRoad.parentConnection.GetComponent<AbstractRoad>())
        {
            List<Vector3> parentPoints = abstractRoad.parentConnection.GetComponent<AbstractRoad>().points;
            Vector3 lineDirectionParent;

            //Обработка ошибки выхода за пределы массива точек дороги-родителя при зацикливании дорог
            try
            {
                lineDirectionParent = (parentPoints[^1] - parentPoints[^2]).normalized;
            }
            catch (ArgumentOutOfRangeException e)
            {
                abstractRoad.parentConnection.GetComponent<AbstractRoad>().BuildRoad(true);
                parentPoints = abstractRoad.parentConnection.GetComponent<AbstractRoad>().points;
                lineDirectionParent = (parentPoints[^1] - parentPoints[^2]).normalized;
            }

            AddVertexes(parentPoints[^1], lineDirectionParent);
        }
    }

    // Рассчитывает координаты точек излома дороги
    private void CalculateMeshVertexPoints()
    {
        CalculateVertexPoints(abstractRoad.points[0], abstractRoad.points[1]);

        if (!abstractRoad.isStraight)
            for (int i = 0; i < abstractRoad.points.Count - 1; i++)
                CalculateVertexPoints(abstractRoad.points[i], abstractRoad.points[i + 1]);

        Vector3 lineDirection = (abstractRoad.points[^1] - abstractRoad.points[^2]).normalized;
        AddVertexes(abstractRoad.points[^1], lineDirection);
    }

    // Рассчитывает направление для ширины дороги
    private void CalculateVertexPoints(Vector3 a, Vector3 b)
    {
        Vector3 lineDirection = (b - a).normalized;

        AddVertexes(a, lineDirection);
    }

    // Добавляет координаты точек "излома" дороги
    private void AddVertexes(Vector3 a, Vector3 lineDirection)
    {
        Vector3 v1 = a + Quaternion.Euler(0, -90, 0) * lineDirection * width / 2;
        Vector3 v2 = a + Quaternion.Euler(0, +90, 0) * lineDirection * width / 2;

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

            for (int k = 0; k < 3; k++) 
                triangles[i * 3 + k] = j++;
            i++;

            for (int k = 0; k < 3; k++) 
                triangles[i * 3 + k] = --j;
        }

        mesh.triangles = triangles;

        MeshCollider mc = GetComponent<MeshCollider>();
        if (abstractRoad.startPost.transform.position != abstractRoad.endPost.transform.position && v.Length != 0)
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
