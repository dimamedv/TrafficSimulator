using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static GlobalSettings;

public abstract class AbstractRoad : MonoBehaviour
{
    public GameObject startPost; // Ñòàðòîâàÿ òî÷êà
    public GameObject endPost; // Êîíå÷íàÿ òî÷êà
    public GameObject formingPoint; // ?????????? ??????? ?????
    public GameObject parentConnection; // Ðîäèòåëü
    public GameObject childConnection; // Ðåáåíîê

    public List<Vector3> points; // Òî÷êè, ÷åðåç êîòîðûå ïðîõîäèò àâòîìîáèëü
    public List<float> prefixSumSegments; // Ïðåôèêñíûå ñóììû äëèí ñåãìåíòîâ äîðîãè
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