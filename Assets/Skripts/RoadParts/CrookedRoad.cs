using System;
using System.Collections.Generic;
using UnityEngine;
using static MyMath;
using static GlobalSettings;

public class CrookedRoad : AbstractRoad
{
    public int details; // Количество деталей дороги
    private int _curDetails;
    public bool isStraight; // Прямая ли дорога
    public bool debugRoad; // Для дебага дороги
    public List<Vector3> _vertexRoad = new List<Vector3>(); // Вершины излома дороги


    public new void Start()
    {
        base.Start();
        _curFormingPointPosition = formingPoint.transform.position;
    }

    protected override void BuildRoad(bool endIteration = true)
    {
        RebuildGrid();
        
        if (childConnection && childConnection.GetComponent<CrookedRoad>() && !endIteration)
        {
            childConnection.GetComponent<CrookedRoad>().BuildRoad();
        }

        // Подготавливаем "почву" для построения дороги
        ClearLists();
        if (isStraight) 
            MakeStraight();
        else 
            _curDetails = details;

        CheckoutChildPost();
        CheckoutParentPost();
        
        // Строим ВСЕ вершины, на основе которых будем строить меши
        CalculateQuadraticBezierCurve(startPost.transform.position, formingPoint.transform.position,
            endPost.transform.position);
        CalculateMeshVertexPoints();

        // Визуализируем все, что только можно визуализировать
        if (debugRoad) 
            ShowDebugPoints();
        
        CreateMesh();

        // Остаточные действия
        CalculateLengthOfRoadSections();
        _curFormingPointPosition = formingPoint.transform.position;
        
        if (childConnection && childConnection.GetComponent<CrookedRoad>() && !endIteration)
            childConnection.GetComponent<CrookedRoad>().BuildRoad();
        if (parentConnection && parentConnection.GetComponent<CrookedRoad>() && !endIteration)
            parentConnection.GetComponent<CrookedRoad>().BuildRoad();

        //if (parentConnection == null)
            //gameObject.AddComponent<CarSpawner>();
    }

    // Обнуляет все списки
    private void ClearLists()
    {
        points.Clear();
        _vertexRoad.Clear();
        prefixSumSegments.Clear();
    }

    private void MakeStraight()
    {
        _curDetails = 1;
        formingPoint.transform.position =
            GetMidPoint(startPost.transform.position, endPost.transform.position);
    }

    // Визуализация Дебаг точек
    private void ShowDebugPoints()
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

    // Рассчет координат точек Безье
    private void CalculateQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
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

    // 
    private void CalculateMeshVertexPoints()
    {
        if (parentConnection && parentConnection.GetComponent<CrookedRoad>())
        {
            List<Vector3> parentPoints = parentConnection.GetComponent<CrookedRoad>().points;
            Vector3 lineDirectionParent;
            
            //Обработка ошибки выхода за пределы массива точек дороги-родителя при зацикливании дорог
            try
            {
                lineDirectionParent = (parentPoints[^1] - parentPoints[^2]).normalized;
            }
            catch (ArgumentOutOfRangeException e)
            {
                parentConnection.GetComponent<CrookedRoad>().BuildRoad(true);
                parentPoints = parentConnection.GetComponent<CrookedRoad>().points;
                lineDirectionParent = (parentPoints[^1] - parentPoints[^2]).normalized;
            }
            
            AddVertexes(parentPoints[^1], lineDirectionParent);
        }
        
        CalculateVertexPoints(points[0], points[1]);

        if (!isStraight)
            for (int i = 0; i < points.Count - 1; i++)
                CalculateVertexPoints(points[i], points[i + 1]);
        
        Vector3 lineDirection = (points[^1] - points[^2]).normalized;
        AddVertexes(points[^1], lineDirection);
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
        Vector3 v1 = a + Quaternion.Euler(0, -90, 0) * lineDirection * width;
        Vector3 v2 = a + Quaternion.Euler(0, +90, 0) * lineDirection * width;
        
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
        if (startPost.transform.position != endPost.transform.position)
        {
            mc.enabled = true;
            mc.sharedMesh = mesh;
        }
        else
        {
            mc.enabled = false;
        }
    }

    // Рассчитывает длину дороги, заполняя массив префиксных сумм
    private void CalculateLengthOfRoadSections()
    {
        prefixSumSegments.Add(0.0f);
        for (int i = 0; i < points.Count - 1; i++)
            prefixSumSegments.Add(prefixSumSegments[i] + getDistance(points[i], points[i + 1]));
    }

    // Возвращает истину, если одна из точек сменила сове положение. Ложь в ином случае.
    protected override bool NeedsRebuild()
    {
        var formingPosition = formingPoint.transform.position;
        var startPosition = startPost.transform.position;
        var endPosition = endPost.transform.position;
        return points.Count == 0
               || points[0] != startPosition
               || points[^1] != endPosition
               || !isStraight && formingPosition != _curFormingPointPosition
               || isStraight && GetMidPoint(startPosition, endPosition) != formingPosition;
    }
}