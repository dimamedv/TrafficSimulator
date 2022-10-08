using System;
using System.Collections.Generic;
using UnityEngine;

public class TemplateRoad : AbstractRoad
{
    public bool debugRoad; // Для дебага дороги
    public GameObject _vertexCubeRed; // Куб для отоборажение одной стороны дороги в режиме дебага
    public GameObject _vertexCubeBLue; // Куб для отоборажение одной стороны дороги в режиме дебага
    public GameObject _bezierCubeGreen; // Куб для отображения точек центра дороги в режиме дебага
    public int _countLanes;
    public List<Vector3> _vertexRoad = new List<Vector3>(); // Вершины излома дороги (для полигонов)
    public Dictionary<string, GameObject> DictOfSimpleRoads = new Dictionary<string, GameObject>();

    public void Awake()
    {
        roadList.Add(gameObject);
    }

    public new void Start()
    {
        base.Start();
        _curFormingPointPosition = formingPoint.transform.position;
    }

    void LateUpdate()
    {
        if (NeedsRebuild())
        {
            BuildRoad(false);
        }
    }

    // Возвращает истину, если одна из точек сменила сове положение. Ложь в ином случае.
    protected bool NeedsRebuild()
    {
        var formingPosition = formingPoint.transform.position;
        var startPosition = startPost.transform.position;
        var endPosition = endPost.transform.position;
        return points.Count == 0
               || points[0] != startPosition
               || points[^1] != endPosition
               || !isStraight && formingPosition != _curFormingPointPosition
               || isStraight && MyMath.GetMidPoint(startPosition, endPosition) != formingPosition;
    }

    public override void BuildRoad(bool endIteration = true)
    {
        RebuildGrid();
        if (!endIteration && childConnection && childConnection.GetComponent<TemplateRoad>())
            childConnection.GetComponent<TemplateRoad>().BuildRoad();
        CheckoutChildPost();
        CheckoutParentPost();

        // Подготавливаем "почву" для построения дороги
        ClearLists();
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
        ClearSimpleRoads();

        // Строим ВСЕ вершины, на основе которых будем строить меши
        RebuildParents();
        CalculateVertexPoints();
        BuildSimpleRoads();

        // Визуализируем все, что только можно визуализировать
        if (debugRoad) 
            ShowDebugPoints();
        
        CreateMesh();

        // Остаточные действия
        //CalculateLengthOfRoadSections();
        _curFormingPointPosition = formingPoint.transform.position;
        
        if (childConnection && childConnection.GetComponent<TemplateRoad>() && !endIteration)
            childConnection.GetComponent<TemplateRoad>().BuildRoad();
        if (parentConnection && parentConnection.GetComponent<TemplateRoad>() && !endIteration)
            parentConnection.GetComponent<TemplateRoad>().BuildRoad();

        //if (parentConnection == null)
            //gameObject.AddComponent<CarSpawner>();
    }

    // Обнуляет все списки
    private void ClearLists()
    {
        points.Clear();
        _vertexRoad.Clear();
    }

    private void ClearSimpleRoads()
    {
        for (int i = 0; i < gameObject.transform.Find("SimpleRoads").childCount; i++)
        {
            gameObject.transform.Find("SimpleRoads").GetChild(i).GetComponent<SimpleRoad>().points.Clear();
            for (int j = 0; j < points.Count; j++)
            {
                gameObject.transform.Find("SimpleRoads").GetChild(i).GetComponent<SimpleRoad>().points.Add(Vector3.zero);
            }
            gameObject.transform.Find("SimpleRoads").GetChild(i).GetComponent<SimpleRoad>().prefixSumSegments = new List<float>();
        }
    }

    // Перестраивает родителей
    private void RebuildParents()
    {
        if (parentConnection && parentConnection.GetComponent<TemplateRoad>())
        {
            List<Vector3> parentPoints = parentConnection.GetComponent<TemplateRoad>().points;
            Vector3 lineDirectionParent;

            //Обработка ошибки выхода за пределы массива точек дороги-родителя при зацикливании дорог
            try
            {
                lineDirectionParent = (parentPoints[^1] - parentPoints[^2]).normalized;
            }
            catch (ArgumentOutOfRangeException e)
            {
                parentConnection.GetComponent<TemplateRoad>().BuildRoad(true);
                parentPoints = parentConnection.GetComponent<TemplateRoad>().points;
                lineDirectionParent = (parentPoints[^1] - parentPoints[^2]).normalized;
            }

            AddMeshVertexes(parentPoints[^1], lineDirectionParent);
        }
    }

    // Рассчитывает координаты точек излома дороги
    private void CalculateVertexPoints()
    {
        Vector3 lineDirection;
        for (int pointIndex = 0; pointIndex < points.Count - 1; pointIndex++)
        {
            lineDirection = CalculateLineDirection(points[pointIndex + 1], points[pointIndex]);
            AddRoadVertexes(points[pointIndex], lineDirection, pointIndex);
            AddMeshVertexes(points[pointIndex], lineDirection);
        }
        
        lineDirection = CalculateLineDirection(points[^1], points[^2]);
        AddRoadVertexes(points[^1], lineDirection, points.Count - 1);
        AddMeshVertexes(points[^1], lineDirection);
    }

    // Рассчитывает направление для ширины дороги
    private Vector3 CalculateLineDirection(Vector3 a, Vector3 b)
    {
        return (a - b).normalized;

        //AddVertexes(a, lineDirection);
    }

    // Добавляет в "простые дороги" координаты точек, по которым в будущем будет ехать машина
    private void AddRoadVertexes(Vector3 a, Vector3 lineDirection, int pointIndex)
    {

        if (_countLanes == 1)
        {
            DictOfSimpleRoads["Right0"].GetComponent<SimpleRoad>().points[0] = a;
        }
        else
        {
            for (int Lane = 0; Lane < _countLanes / 2; Lane++)
            {
                Debug.Log(pointIndex);
                DictOfSimpleRoads["Right" + Lane].GetComponent<SimpleRoad>().points[pointIndex] = 
                    a + Quaternion.Euler(0, +90, 0) * lineDirection * GlobalSettings.width * (Lane + GlobalSettings.width / 4);
                DictOfSimpleRoads["Left" + Lane].GetComponent<SimpleRoad>().points[^(pointIndex+1)] =
                    a + Quaternion.Euler(0, -90, 0) * lineDirection * GlobalSettings.width * (Lane + GlobalSettings.width / 4);
            }
        }
    }

    // Добавляет координаты точек "излома" дороги
    private void AddMeshVertexes(Vector3 a, Vector3 lineDirection)
    {
        Vector3 v1 = a + Quaternion.Euler(0, -90, 0) * lineDirection * GlobalSettings.width * _countLanes / 2;
        Vector3 v2 = a + Quaternion.Euler(0, +90, 0) * lineDirection * GlobalSettings.width * _countLanes / 2;
        
        _vertexRoad.Add(v1);
        _vertexRoad.Add(v2);
    }

    // Запускает метод BuildRoad для всех SimpleRoad для этой дороги.
    private void BuildSimpleRoads()
    {
        for (int i = 0; i < _countLanes; i++)
        {
            if (i % 2 == 0)
                DictOfSimpleRoads["Right" + i / 2].GetComponent<SimpleRoad>().BuildRoad();
            else
                DictOfSimpleRoads["Left" + i / 2].GetComponent<SimpleRoad>().BuildRoad();
        }
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
        for (int i = 0; i < points.Count; i++) 
            Instantiate(_bezierCubeGreen, points[i], new Quaternion());
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

    // Включает видимость всех формирующих точек объекта _gameObject
    public static void TurnOnPoints(GameObject _gameObject)
    {
        for (int i = 0; i < _gameObject.transform.childCount - 1; i++)
        {
            _gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
            _gameObject.transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
        }
    }

    // Выключает видимость всех формирующих точек объекта _gameObject
    public static void TurnOffPoints(GameObject _gameObject)
    {
        for (int i = 0; i < _gameObject.transform.childCount - 1; i++)
        {
            _gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
            _gameObject.transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;
        }
    }

    protected void CheckoutChildPost()
    {
        childConnection = null;
        foreach (var checkedRoad in roadList)
        {
            if (checkedRoad.gameObject != gameObject && 
                checkedRoad.GetComponent<TemplateRoad>().startPost.transform.position == endPost.transform.position)
            {
                ConnectFromParentToChild(checkedRoad.GetComponent<TemplateRoad>());
            }
        }
    }

    protected void CheckoutParentPost()
    {
        parentConnection = null;
        foreach (var checkedRoad in roadList)
        {
            if (checkedRoad.GetComponent<TemplateRoad>().endPost.transform.position == startPost.transform.position &&
                checkedRoad.gameObject != gameObject)
            {
                ConnectFromChildToParent(checkedRoad.GetComponent<TemplateRoad>());
            }
        }
    }

    private void ConnectFromParentToChild(TemplateRoad newChildRoad)
    {
        childConnection = newChildRoad.gameObject;
        newChildRoad.parentConnection = gameObject;
    }

    private void ConnectFromChildToParent(TemplateRoad newParentRoad)
    {
        parentConnection = newParentRoad.gameObject;
        newParentRoad.childConnection = gameObject;
    }


}