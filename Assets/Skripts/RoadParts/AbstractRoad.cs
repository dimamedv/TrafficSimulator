using System.Collections;
using System.Collections.Generic;
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

    public GameObject _vertexCubeRed;
    public GameObject _vertexCubeBLue;
    public GameObject _bezierCubeGreen;


    public void Awake()
    {
        transform.position = Vector3.zero;
        startPost = transform.GetChild(0).gameObject;
        endPost = transform.GetChild(1).gameObject;

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


    protected void RebuildGridByPoint(ref GameObject t)
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

    
    protected abstract void BuildRoad();
    protected abstract bool NeedsRebuild();
}