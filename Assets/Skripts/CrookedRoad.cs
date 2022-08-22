 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class CrookedRoad : AbstractRoad
{
    // ���������� ������� �����
    public Transform formingPointTransform;
    
    // ���������� ���������� ������ (�����������)
    public int details;
    
    // ������� ������
    private List<Vector3> _vertexRoad;

    // ����� ��������� ������
    private List<float> lengthSegments;

    // ���������� ����� ���� ��������� ������
    private List<float> prefixSumSegments;


    // ���������� ������ ����� �� ���� �����������
    private void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
    {
        float t = 0f;
        Vector3 B;
        for (int i = 0; i < details; i++)
        {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            _roadPoints.Add(B);
            t += (1 / (float)details);
        }
        _roadPoints.Add(point2);
    }
 
    private void getVertexPoints()
    {
        GetEndpoints(_roadPoints[0], _roadPoints[1]);

        for (int i = 1; i < details; i++)
            GetBendOfRoad(_roadPoints[i - 1], _roadPoints[i], _roadPoints[i + 1]);

        GetEndpoints(_roadPoints[_roadPoints.Count - 1], _roadPoints[_roadPoints.Count - 2]);
    }

    private float getDistance(Vector3 v1, Vector3 v2) { 
        double x = v1.x - v2.x;
        double z = v1.z - v2.z;
        return (float)Math.Sqrt(x * x + z * z);
    }

    /* ��������� ������� ������� ����� � List vertexRoad, ����� ���������
    ** a - ������ ����� ������������� ������ �����
    ** b - ������ (���������) ����� ������������� ������ �����
    ** offset - �������� ������� ������, ������������ a
    */
    private void addOuterVertexFirst(Vector3 a, Vector3 b, Vector3 offset)
    {
        Vector3 v1 = a + offset;
        Vector3 v2 = a - offset;

        if (getDistance(v1, b) > getDistance(v2, b))
        {
            _vertexRoad.Add(v1);
            _vertexRoad.Add(v2);
        }
        else
        {
            _vertexRoad.Add(v2);
            _vertexRoad.Add(v1);
        }
    }

    // ���������� ���������� ������ ��� ����� ������ � ���� vertexRoad
    private void GetEndpoints(Vector3 a, Vector3 b)
    {
        Vector3 delta = b - a;
        double arctgA = Math.Atan(delta.x / delta.z);
        // ������ ����� ����� ���������
        Vector3 offset = new Vector3((float)Math.Cos(-arctgA) * roadWidth, 0f, (float)Math.Sin(-arctgA) * roadWidth);
        addOuterVertexFirst(a, b, offset);
    }

    // ���������� ���������� ������ �� ����� ������ ������ � ���� vertexRoad
    private void GetBendOfRoad(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 delta = b - a;
        double arctgA = Math.Atan(delta.x / delta.z);
        Vector3 AB = b - a;
        Vector3 BC = b - c;
        double lenAB = Math.Sqrt(AB.x * AB.x + AB.z * AB.z);
        double lenBC = Math.Sqrt(BC.x * BC.x + BC.z * BC.z);
        // ���������� ���� p1p2p3 �������� �� ���
        double arccos = Math.Acos((AB.x * BC.x + AB.z * BC.z) / (lenAB * lenBC)) / 2;
        // ������ ����� ����� ���������
        Vector3 offset = new Vector3((float)Math.Cos(arccos - arctgA + Math.PI / 2) * roadWidth, 0f, (float)Math.Sin(arccos - arctgA + Math.PI/2) * roadWidth);

        addOuterVertexFirst(b, c, offset);
    }

    private void CreateMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        // ������ ����� ����� ���������
        Vector3[] V = new Vector3[_vertexRoad.Count];
        for (int i = 0; i < _vertexRoad.Count; i++) V[i] = _vertexRoad[i];
        mesh.vertices = V;

        // ���������� ������� ����� ����� ���������� ������ � ������������ �� ���������� �������������
        // � ��������. � ��� ��� �������� ����� ������� � ���� ������, �� ��������� ��� �� 2
        int[] triangles = new int[_roadPoints.Count * 3 * 2 * 2];

        // ������� ������ ������ ������������ ������ ����������, � ����� �������
        for (int i = 0; i < (_roadPoints.Count - 1) * 2 * 2; i++)
        {
            // ����� ������������
            int j = i / 2;
            // ����������� ����� ������� ������������
            for (int k = 0; k < 3; k++) triangles[i * 3 + k] = j++;
            i++;
            // ����������� ������ ������� ������������
            for (int k = 0; k < 3; k++) triangles[i * 3 + k] = --j;
        }

        mesh.triangles = triangles;
    }

    protected override void BuildRoad()
    {
        _roadPoints = new List<Vector3>();
        _vertexRoad = new List<Vector3>();
        
        // ������� ������ �� ����������� ����� ������ �����
        DrawQuadraticBezierCurve(_startPostTransform.position, formingPointTransform.position, _endPostTransform.position);

        // �� ��� �������� ���������� �����, ������� �������� �������� ������
        getVertexPoints();

        // �� ���� ����������� ������� ��� ������
        CreateMesh();

        // ������������ ����� ������ ������ ������
        getLengthOfRoadSections();
    }

    private void getLengthOfRoadSections()
    {
        lengthSegments = new List<float>(_roadPoints.Count);
        prefixSumSegments = new List<float>(_roadPoints.Count + 1);

        for (int i = 0; i < _roadPoints.Count; i++)
        {
            lengthSegments[i] = getDistance(_roadPoints[i], _roadPoints[i + 1]);
            prefixSumSegments[i + 1] = prefixSumSegments[i] + lengthSegments[i];
        }
    }
}
