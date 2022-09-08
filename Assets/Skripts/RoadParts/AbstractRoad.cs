using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static GlobalSettings;

public abstract class AbstractRoad : MonoBehaviour
{
    public GameObject parentConnection; // Соединение с родителем
    public GameObject childConnection; // Соединение с ребенком
    public GameObject _vertexCubeRed; // Куб для отоборажение одной стороны дороги в режиме дебага
    public GameObject _vertexCubeBLue; // Куб для отоборажение одной стороны дороги в режиме дебага
    public GameObject _bezierCubeGreen; // Куб для отображения точек центра дороги в режиме дебага
    public List<Vector3> points; // Массив центральных точек (Безье), по которым едет машина
    public List<float> prefixSumSegments; // Массив префиксных сумм. Последний элемент - длина всей дороги
    public List<GameObject> carsOnThisRoad; // Массив машин, который в данный момент едут по этой дороге

    protected static List<GameObject> RoadList; // Массив всех дорог
    public GameObject startPost; // Стартовая точка
    public GameObject endPost; // Конечная точка
    public GameObject formingPoint; // Формирующая точка
    public Vector3 _curFormingPointPosition; // "Указатель" на формирующую точку, чтобы отслеживать перемещение


    public void Start()
    {
        startPost = transform.GetChild(0).gameObject;
        endPost = transform.GetChild(1).gameObject;
        formingPoint = transform.GetChild(2).gameObject;

        RoadList ??= new List<GameObject>();
        RoadList.Add(gameObject);
        
        BuildRoad();
    }

    void FixedUpdate()
    {
        if (NeedsRebuild())
        {
            BuildRoad();
        }
    }

    public void OnDestroy()
    {
        RoadList.Remove(gameObject);
    }


    private void RebuildGridByPoint(ref GameObject t)
    {
        var position = t.transform.position;
        position = new Vector3(
            RebuildGridByAxis(position.x),
            0.0f,
            RebuildGridByAxis(position.z));
        t.transform.position = position;
    }

    private float RebuildGridByAxis(float x)
    {
        float remains = x % GlobalSettings.gridStep;
        if (remains < gridStep / 2)
            return x - remains;
        else
            return x - remains + gridStep;
    }
    
    // Подстраивает точки под сетку
    protected  void RebuildGrid()
    {
        RebuildGridByPoint(ref startPost);
        RebuildGridByPoint(ref endPost);
    }

    protected void CheckoutChildPost()
    {
        childConnection = null;
        foreach (var checkedRoad in RoadList)
        {
            if (checkedRoad.GetComponent<AbstractRoad>().startPost.transform.position == endPost.transform.position &&
                checkedRoad.gameObject != gameObject)
            {
                ConnectFromParentToChild(checkedRoad.GetComponent<AbstractRoad>());
            }
        }
    }
    
    protected void CheckoutParentPost()
    {
        parentConnection = null;
        foreach (var checkedRoad in RoadList)
        {
            if (checkedRoad.GetComponent<AbstractRoad>().endPost.transform.position == startPost.transform.position &&
                checkedRoad.gameObject != gameObject)
            {
                ConnectFromChildToParent(checkedRoad.GetComponent<AbstractRoad>());
            }
        }
    }

    private void ConnectFromParentToChild(AbstractRoad newChildRoad)
    {
        childConnection = newChildRoad.gameObject;
        newChildRoad.parentConnection = gameObject;
    }

    private void ConnectFromChildToParent(AbstractRoad newParentRoad)
    {
        parentConnection = newParentRoad.gameObject;
        newParentRoad.childConnection = gameObject;
    }
    
    protected abstract void BuildRoad();
    protected abstract bool NeedsRebuild();
}