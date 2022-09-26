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
    public List<Vector3> points = new List<Vector3>(); // Массив центральных точек (Безье), по которым едет машина
    public List<float> prefixSumSegments = new List<float>(); // Массив префиксных сумм. Последний элемент - длина всей дороги
    public List<GameObject> carsOnThisRoad; // Массив машин, который в данный момент едут по этой дороге

    public static List<GameObject> RoadList = new List<GameObject>(); // Массив всех дорог
    public GameObject startPost; // Стартовая точка
    public GameObject endPost; // Конечная точка
    public GameObject formingPoint; // Формирующая точка
    public Vector3 _curFormingPointPosition; // "Указатель" на формирующую точку, чтобы отслеживать перемещение


    public void Awake()
    {
        startPost = transform.GetChild(0).gameObject;
        endPost = transform.GetChild(1).gameObject;
        formingPoint = transform.GetChild(2).gameObject;
        RoadList.Add(gameObject);
    }

    public void Start()
    {
        BuildRoad(false);
    }

    void LateUpdate()
    {
        if (NeedsRebuild())
        {
            BuildRoad(false);
        }
    }

    public void OnDestroy()
    {
        RoadList.Remove(gameObject);
    }

    // Подстраивает точки под сетку
    protected void RebuildGrid()
    {
        RebuildPointByGrid(startPost.transform);
        RebuildPointByGrid(endPost.transform);
    }

    public static void RebuildPointByGrid(Transform t)
    {
        t.transform.position = new Vector3(
            RebuildAxisByGrid(t.transform.position.x),
            0.0f,
            RebuildAxisByGrid(t.transform.position.z));
    }

    private static float RebuildAxisByGrid(float x)
    {
        float remains = x % GlobalSettings.gridStep;
        float isNegative = (x < 0 ? -1 : 1);

        if (isNegative * remains < gridStep / 2)
            return x - remains;
        else
            return x - remains + isNegative * gridStep;
    }

    
    protected abstract void BuildRoad(bool endIteration = true);
    protected abstract bool NeedsRebuild();
}