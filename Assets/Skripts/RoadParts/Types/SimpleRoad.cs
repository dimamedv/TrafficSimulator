using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GlobalSettings;

public class SimpleRoad : AbstractRoad
{
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
        if (!endIteration)
            RebuildRoadPostsPositions();
        CheckCrossRoadEntranceState();
        
        if (!endIteration && childConnection && childConnection.GetComponent<SimpleRoad>())
            childConnection.GetComponent<SimpleRoad>().BuildRoad();
        
        CheckoutChildConnection();
        CheckoutParentConnection();
        
        ClearLists();
        GetPoints();

        gameObject.GetComponent<AbstractVisualization>().RenderingRoad();

        // Остаточные действия
        CalculateLengthOfRoadSections();
        _curFormingPointPosition = formingPoint.transform.position;
        
        if (childConnection && childConnection.GetComponent<SimpleRoad>() && !endIteration)
            childConnection.GetComponent<SimpleRoad>().BuildRoad();
        if (parentConnection && parentConnection.GetComponent<SimpleRoad>() && !endIteration)
            parentConnection.GetComponent<SimpleRoad>().BuildRoad();
    }

    private void GetPoints()
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
        prefixSumSegments.Clear();
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