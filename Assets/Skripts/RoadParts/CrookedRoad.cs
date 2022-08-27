using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEditor;
using System.Runtime.ConstrainedExecution;
using UnityEditor.IMGUI.Controls;
using static MyMath;

public class CrookedRoad : AbstractRoad
{
    public GameObject formingPoint; // ���������� ������� �����
    public int details; // �������� ���������� ���������� ������
    public bool isStraight;
    
    
    private List<Vector3> _vertexRoad; // ������� ������
    private Vector3 _curFormingPointPosition;
    private int _curDetails; // �������������� ���������� ���������� ������


    public new void Awake()
    {
        base.Awake();

        _curDetails = details;
        _curFormingPointPosition = formingPoint.transform.position;
    }


    protected override void BuildRoad()
    {
        points = new List<Vector3>();
        _vertexRoad = new List<Vector3>();
        angles = new List<Vector3>();
        prefixSumSegments = new List<float>();

        if (isStraight)
        {
            _curDetails = 1;
            formingPoint.transform.position =
                GetMidPoint(startPost.transform.position, endPost.transform.position);
        }
        else
        {
            _curDetails = details;
        }

        formingPoint = transform.GetChild(2).gameObject;
        RebuildGrid();

        DrawQuadraticBezierCurve(startPost.transform.position, formingPoint.transform.position,
            endPost.transform.position);
        CalculateMeshVertexPoints();
        CreateMesh();
        CalculateLengthOfRoadSections();

        _curFormingPointPosition = formingPoint.transform.position;
    }


    // ���������� ������ ����� �� ���� �����������
    private void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
    {
        float t = 0f;
        Vector3 B;
        for (int i = 0; i < _curDetails; i++)
        {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            points.Add(B);
            t += (1 / (float)_curDetails);
        }

        points.Add(point2);
    }

    private void CalculateMeshVertexPoints()
    {
        GetEndPoints(points[0], points[1]);

        for (int i = 1; i < _curDetails; i++)
            GetBendOfRoad(points[i - 1], points[i], points[i + 1]);

        GetEndPoints(points[^1], points[^2]);
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
        int[] triangles = new int[points.Count * 3 * 2 * 2];

        for (int i = 0; i < (points.Count - 1) * 2 * 2; i++)
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


    /* ��������� ������� ������� ����� � List vertexRoad, ����� ���������
    ** a - ������ ����� ������������� ������ �����
    ** b - ������ (���������) ����� ������������� ������ �����
    ** offset - �������� ������� ������, ������������ a
    */
    private void addOuterVertexFirst(Vector3 a, Vector3 b, double alfa)
    {
        float cos = (float)Math.Cos(alfa);
        float sin = (float)Math.Sin(alfa);
        // ������ ����� ����� ���������
        Vector3 offset = new Vector3(cos * width, 0f, sin * width);
        Vector3 v1 = a + offset;
        Vector3 v2 = a - offset;

        if (getDistance(v1, b) > getDistance(v2, b))
        {
            _vertexRoad.Add(v1);
            _vertexRoad.Add(v2);
            angles.Add(new Vector3(sin, 0.0f, -cos));
        }
        else
        {
            _vertexRoad.Add(v2);
            _vertexRoad.Add(v1);
            angles.Add(new Vector3(-sin, 0.0f, cos));
        }
    }


    // ���������� ���������� ������ ��� ����� ������ � ���� vertexRoad
    private void GetEndPoints(Vector3 a, Vector3 b)
    {
        Vector3 delta = b - a;
        double arctgA = -Math.Atan(delta.x / delta.z);
        addOuterVertexFirst(a, b, arctgA);
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
        double alfa = arccos - arctgA + Math.PI / 2;
        addOuterVertexFirst(b, c, alfa);
    }


    private void CalculateLengthOfRoadSections()
    {
        prefixSumSegments.Add(0.0f);

        for (int i = 0; i < points.Count - 1; i++)
            prefixSumSegments.Add(prefixSumSegments[i] + getDistance(points[i], points[i + 1]));
    }


    protected override bool NeedsRebuild()
    {
        var formingPosition = formingPoint.transform.position;
        var startPosition = startPost.transform.position;
        var endPosition = endPost.transform.position;
        return points[0] != startPosition
               || points[^1] != endPosition
               || !isStraight && formingPosition != _curFormingPointPosition
               || isStraight && GetMidPoint(startPosition, endPosition) != formingPosition;
    }
}