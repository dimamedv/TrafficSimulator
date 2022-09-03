using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static GlobalSettings;

public abstract class AbstractRoad : MonoBehaviour
{
    public GameObject startPost; // Стартовая точка
    public GameObject endPost; // Конечная точка
    public GameObject formingPoint; // ?????????? ??????? ?????
    public GameObject parentPost; // Родитель
    public GameObject childPost; // Ребенок

    public List<Vector3> points; // Точки, через которые проходит автомобиль
    public List<float> prefixSumSegments; // Префиксные суммы длин сегментов дороги
    public List<GameObject> carsOnThisRoad;
    protected static List<GameObject> RoadList;

    public GameObject _vertexCubeRed;
    public GameObject _vertexCubeBLue;
    public GameObject _bezierCubeGreen;


    public void Start()
    {
    }

    public void Awake()
    {
        transform.position = Vector3.zero;
        startPost = transform.GetChild(0).gameObject;
        endPost = transform.GetChild(1).gameObject;

        RoadList ??= new List<GameObject>();
        RoadList.Add(gameObject);

        BuildRoad();
    }

    public void Awake(GameObject startPost, GameObject endPost)
    {
        transform.position = Vector3.zero;
        startPost = transform.GetChild(0).gameObject;
        endPost = transform.GetChild(1).gameObject;

        BuildRoad();
    }

    void FixedUpdate()
    {
        transform.position = Vector3.zero;
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
    
    protected  void RebuildGrid()
    {
        RebuildGridByPoint(ref startPost);
        RebuildGridByPoint(ref endPost);
    }

    protected void CheckoutChildPost()
    {
        childPost = null;
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
        parentPost = null;
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
        childPost = newChildRoad.gameObject;
        newChildRoad.parentPost = gameObject;
    }

    private void ConnectFromChildToParent(AbstractRoad newParentRoad)
    {
        parentPost = newParentRoad.gameObject;
        newParentRoad.childPost = gameObject;
    }
    
    protected abstract void BuildRoad();
    protected abstract bool NeedsRebuild();
}