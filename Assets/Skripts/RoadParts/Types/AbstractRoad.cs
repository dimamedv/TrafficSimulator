using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class AbstractRoad : MonoBehaviour
{
    public int details; // Количество деталей дороги
    public bool isStraight; // Прямая ли дорога
    public GameObject parentConnection; // Соединение с родителем
    public GameObject childConnection; // Соединение с ребенком
    public List<Vector3> points = new List<Vector3>(); // Массив центральных точек (Безье), по которым едет машина
    public List<GameObject> carsOnThisRoad; // Массив машин, который в данный момент едут по этой дороге

    public static List<GameObject> RoadList = new List<GameObject>(); // Массив всех дорог
    public GameObject startPost; // Стартовая точка
    public GameObject endPost; // Конечная точка
    public GameObject formingPoint; // Формирующая точка
    public Vector3 _curFormingPointPosition; // "Указатель" на формирующую точку, чтобы отслеживать перемещение

    public virtual void Awake()
    {
        startPost = transform.GetChild(0).gameObject;
        endPost = transform.GetChild(1).gameObject;
        formingPoint = transform.GetChild(2).gameObject;
    }

    void LateUpdate()
    {
        if (NeedsRebuild())
        {
            BuildRoad(false);
        }
    }

    abstract protected bool NeedsRebuild();

    // Рассчет координат точек Безье
    protected void CalculateQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2, int details)
    {
        float t = 0f;
        Vector3 B;
        for (int i = 0; i < details; i++)
        {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            points.Add(B);
            t += (1 / (float)details);
        }

        points.Add(point2);
    }

    // Подстраивает точки под сетку
    protected void RebuildRoadPostsPositions()
    {
        List<Vector3> endPostPositions = new List<Vector3>();
        List<Vector3> startPostPositions = new List<Vector3>();

        for (int i = 0; i < RoadList.Count; i++)
        {
            if (RoadList[i] == gameObject)
                continue;

            endPostPositions.Add(RoadList[i].GetComponent<AbstractRoad>().endPost.transform.position);
            startPostPositions.Add(RoadList[i].GetComponent<AbstractRoad>().startPost.transform.position);
        }
        
        RebuildPostPosition(startPost, endPostPositions);
        RebuildPostPosition(endPost, startPostPositions);
    }

    public void RebuildPostPosition(GameObject post, List<Vector3> positions)
    {
        Vector3 postPosition = post.transform.position;
        for (int i = 0; i < positions.Count; i++)
        {

            if (Vector3.Distance(postPosition, positions[i]) <= 4)
            {
                post.transform.position = positions[i];
                return;
            }
        }
        
        RebuildPointByGrid(post.transform);
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

        if (isNegative * remains < GlobalSettings.gridStep / 2)
            return x - remains;
        else
            return x - remains + isNegative * GlobalSettings.gridStep;
    }


    public abstract void BuildRoad(bool endIteration = true);
}