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
using static GlobalSettings;
using static UnityEditor.PlayerSettings;

public class CrookedRoad : AbstractRoad
{
    public int details; // �������� ���������� ���������� ������
    public bool isStraight;
    public bool debugRoad;

    private List<Vector3> _vertexRoad; // ������� ������
    private Vector3 _curFormingPointPosition;
    private int _curDetails; // �������������� ���������� ���������� ������


    public new void Awake()
    {
        base.Awake();

        formingPoint = GameObject.Find("FormingPoint");
        _curDetails = details;
        _curFormingPointPosition = formingPoint.transform.position;
    }


    protected override void BuildRoad()
    {
        points = new List<Vector3>();
        _vertexRoad = new List<Vector3>();
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

        if (debugRoad)
        {
            for (int i = 0; i < _vertexRoad.Count; i++)
            {
                if (i % 2 == 0)
                    Instantiate(_vertexCubeRed, _vertexRoad[i], new Quaternion());
                else
                    Instantiate(_vertexCubeBLue, _vertexRoad[i], new Quaternion());
            }
            for (int i = 0; i < points.Count; i++) Instantiate(_bezierCubeGreen, points[i], new Quaternion());
        }

        CreateMesh();
        CalculateLengthOfRoadSections();

        _curFormingPointPosition = formingPoint.transform.position;

        CheckoutChildPost();
        CheckoutParentPost();
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
        CalculateVertexPoints(points[0], points[1]);

        for (int i = 0; i <= _curDetails - 1; i++)
            CalculateVertexPoints(points[i], points[i + 1]);
        
        Vector3 lineDirection = (points[^1] - points[^2]).normalized;
        AddVertexes(points[^1], lineDirection);
    }
    
    
    private void AddVertexes(Vector3 a, Vector3 lineDirection)
    {
        Vector3 v1 = a + Quaternion.Euler(0, -90, 0) * lineDirection * width;
        Vector3 v2 = a + Quaternion.Euler(0, +90, 0) * lineDirection * width;
        
        _vertexRoad.Add(v1);
        _vertexRoad.Add(v2);
    }


    private void CreateMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        // ������ ����� ����� ���������
        Vector3[] v = new Vector3[_vertexRoad.Count];
        for (int i = 0; i < _vertexRoad.Count; i++) v[i] = _vertexRoad[i];
        mesh.vertices = v;

        // ���������� ������� ����� ����� ���������� ������ � ������������ �� ���������� �������������
        // � ��������. � ��� ��� �������� ����� ������� � ���� ������, �� ��������� ��� �� 2
        int[] triangles = new int[points.Count * 3 * 2 * 2];

        for (int i = 0; i < points.Count * 2 * 2; i++)
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


    // ���������� ���������� ������ ��� ����� ������ � ���� vertexRoad
    private void CalculateVertexPoints(Vector3 a, Vector3 b)
    {
        Vector3 lineDirection = (b - a).normalized;

        AddVertexes(a, lineDirection);
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