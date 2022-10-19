using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GlobalSettings;

public class SimpleRoad : AbstractRoad
{
    public bool debugRoad; // Для дебага дороги
    public GameObject _vertexCubeRed; // Куб для отоборажение одной стороны дороги в режиме дебага
    public GameObject _vertexCubeBLue; // Куб для отоборажение одной стороны дороги в режиме дебага
    public GameObject _bezierCubeGreen; // Куб для отображения точек центра дороги в режиме дебага
    public List<float> prefixSumSegments = new List<float>(); // Массив префиксных сумм. Последний элемент - длина всей дороги
    public bool createCrossRoadEntrance;
    public GameObject crossRoadEntrancePrefab;
    public GameObject crossRoadEntrance;
    public GameObject templateOwner;

    public override void Awake()
    {
        base.Awake();
        RoadList.Add(gameObject);
    }

    public new void Start()
    {
        base.Start();
        _curFormingPointPosition = formingPoint.transform.position;
    }
    
    public void OnDestroy()
    {
        RoadList.Remove(gameObject);
    }

    public override void BuildRoad(bool endIteration = true)
    {
        RebuildGrid();
        CheckCrossRoadEntranceState();
        
        if (!endIteration && childConnection && childConnection.GetComponent<SimpleRoad>())
            childConnection.GetComponent<SimpleRoad>().BuildRoad();
        
        CheckoutChildConnection();
        CheckoutParentConnection();
        
        ClearLists();
        GetPointsForMesh();

        // Строим ВСЕ вершины, на основе которых будем строить меши
        CalculateMeshVertexPoints();

        // Визуализируем все, что только можно визуализировать
        if (debugRoad) 
            ShowDebugPoints();
        
        CreateMesh();

        // Остаточные действия
        CalculateLengthOfRoadSections();
        _curFormingPointPosition = formingPoint.transform.position;
        
        if (childConnection && childConnection.GetComponent<SimpleRoad>() && !endIteration)
            childConnection.GetComponent<SimpleRoad>().BuildRoad();
        if (parentConnection && parentConnection.GetComponent<SimpleRoad>() && !endIteration)
            parentConnection.GetComponent<SimpleRoad>().BuildRoad();
    }

    private void GetPointsForMesh()
    {
        if (templateOwner == null)
        {
            if (isStraight)
            {
                formingPoint.transform.position =
                    MyMath.GetMidPoint(startPost.transform.position, endPost.transform.position);
                CalculateQuadraticBezierCurve(startPost.transform.position, formingPoint.transform.position,
                    endPost.transform.position, 1);
            }
            else
            {
                CalculateQuadraticBezierCurve(startPost.transform.position, formingPoint.transform.position,
                    endPost.transform.position, details);
            }
        }
        else
        {
            points = templateOwner.GetComponent<TemplateRoad>().GetBezierPointsByIdentifier(name);
            startPost.transform.position = points[0];
            endPost.transform.position = points[^1];
            formingPoint.SetActive(false);
        }
    }

    private void CheckCrossRoadEntranceState()
    {
        if (createCrossRoadEntrance && !transform.Find("CrossRoadEntrance"))
        {
            crossRoadEntrance = Instantiate(crossRoadEntrancePrefab, endPost.transform.position,
                endPost.transform.rotation);
            crossRoadEntrance.transform.SetParent(gameObject.transform);
            crossRoadEntrance.transform.name = "CrossRoadEntrance";
        }
        else if (!createCrossRoadEntrance && transform.Find("CrossRoadEntrance"))
        {
            Destroy(crossRoadEntrance);
            CrossRoadEntrance.EntrancesList.Remove(crossRoadEntrance);
            crossRoadEntrance = null;
        }
    }

    // Обнуляет все списки
    private void ClearLists()
    {
        points.Clear();
        _vertexRoad.Clear();
        prefixSumSegments.Clear();
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

    // Рассчитывает координаты точек излома дороги
    private void CalculateMeshVertexPoints()
    {
        if (parentConnection && parentConnection.GetComponent<SimpleRoad>())
        {
            List<Vector3> parentPoints = parentConnection.GetComponent<SimpleRoad>().points;
            Vector3 lineDirectionParent;
            
            //Обработка ошибки выхода за пределы массива точек дороги-родителя при зацикливании дорог
            try
            {
                lineDirectionParent = (parentPoints[^1] - parentPoints[^2]).normalized;
            }
            catch (ArgumentOutOfRangeException e)
            {
                parentConnection.GetComponent<SimpleRoad>().BuildRoad(true);
                parentPoints = parentConnection.GetComponent<SimpleRoad>().points;
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
            prefixSumSegments.Add(prefixSumSegments[i] + MyMath.getDistance(points[i], points[i + 1]));
    }

    // Включает видимость всех детей объекта _gameObject
    public static void TurnOnKids(GameObject _gameObject)
    {
        for (int i = 0; i < _gameObject.transform.childCount; i++)
        {
            _gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
            _gameObject.transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
        }
    }

    // Выключает видимость всех детей объекта _gameObject
    public static void TurnOffKids(GameObject _gameObject)
    {
        for (int i = 0; i < _gameObject.transform.childCount; i++)
        {
            _gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
            _gameObject.transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;
        }
    }

    protected void CheckoutChildConnection()
    {
        childConnection = null;

        foreach (var checkedCrossRoadEntrance in CrossRoadEntrance.EntrancesList)
        {
            checkedCrossRoadEntrance.GetComponent<CrossRoadEntrance>().parentRoads.Remove(gameObject);
            if (checkedCrossRoadEntrance.GetComponent<CrossRoadEntrance>().transform.position ==
                endPost.transform.position)
            {
                childConnection = checkedCrossRoadEntrance;
                childConnection.GetComponent<CrossRoadEntrance>().parentRoads.Add(gameObject);
                return;
            } 
        }
        
        foreach (var checkedRoad in RoadList)
        {
            if (checkedRoad.GetComponent<SimpleRoad>().startPost.transform.position == endPost.transform.position &&
                checkedRoad.gameObject != gameObject)
            {
                ConnectFromParentToChild(checkedRoad.GetComponent<SimpleRoad>());
            }
        }
    }

    protected void CheckoutParentConnection()
    {
        parentConnection = null;
        
        foreach (var checkedCrossRoadEntrance in CrossRoadEntrance.EntrancesList)
        {
            checkedCrossRoadEntrance.GetComponent<CrossRoadEntrance>().childRoads.Remove(gameObject);
            if (checkedCrossRoadEntrance.GetComponent<CrossRoadEntrance>().transform.position ==
                startPost.transform.position)
            {
                parentConnection = checkedCrossRoadEntrance;
                parentConnection.GetComponent<CrossRoadEntrance>().childRoads.Add(gameObject);
                return;
            }
        }
        
        foreach (var checkedRoad in RoadList)
        {
            if (checkedRoad.GetComponent<SimpleRoad>().endPost.transform.position == startPost.transform.position &&
                checkedRoad.gameObject != gameObject)
            {
                ConnectFromChildToParent(checkedRoad.GetComponent<SimpleRoad>());
            }
        }
    }

    private void ConnectFromParentToChild(SimpleRoad newChildRoad)
    {
        childConnection = newChildRoad.gameObject;
        newChildRoad.parentConnection = gameObject;
    }

    private void ConnectFromChildToParent(SimpleRoad newParentRoad)
    {
        parentConnection = newParentRoad.gameObject;
        newParentRoad.childConnection = gameObject;
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
               || isStraight && MyMath.GetMidPoint(startPosition, endPosition) != formingPosition
               || createCrossRoadEntrance && transform.Find("CrossRoadEntrance") == null
               || !createCrossRoadEntrance && transform.Find("CrossRoadEntrance");
    }           


}