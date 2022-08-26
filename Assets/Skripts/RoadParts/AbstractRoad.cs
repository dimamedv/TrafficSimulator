using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractRoad : MonoBehaviour
{
    // Стартовая точка
    public GameObject _startPost;

    // Конечная точка
    public GameObject _endPost;

    // Родитель
    public GameObject _parentPost;

    // Ребенок
    public GameObject _childPost;

    // Ширина дороги
    public float width;

    // Количество фрагментов дороги (Детализация)
    public int details;

    // Точки, через которые проходит автомобиль
    public List<Vector3> points;

    // Префиксные суммы длин сегментов дороги
    public List<float> prefixSumSegments;

    // Шаг сетки привязки
    public float gridStep;
    
    // Угол до следующей точки. (cosA, 0, sinA)
    public List<Vector3> angles;

    protected Vector3 curStartPosition;
    protected Vector3 curEndPosition;
    
    

    public List<GameObject> carsOnThisRoad;
    
    public void Awake()
    {
        _startPost = transform.GetChild(0).gameObject;
        _endPost = transform.GetChild(1).gameObject;
        curStartPosition = _startPost.transform.position;
        curEndPosition = _endPost.transform.position;

        BuildRoad();
    }

    void FixedUpdate()
    {
        if (isNeedsRebuild())
        {
            BuildRoad();
        }
    }


    protected void RebuildGridByPoint(ref GameObject t) { 
        t.transform.position = new Vector3(
            RebuildGridByAxis(t.transform.position.x),
            0.0f,
            RebuildGridByAxis(t.transform.position.z));
    }

    private float RebuildGridByAxis(float x)
    {
        float remains = x % gridStep;
        if (remains < gridStep / 2) 
            return x - remains;
        else 
            return x - remains + gridStep;
    }

    protected abstract void BuildRoad();
    protected abstract bool isNeedsRebuild();
    protected abstract void RebuildGrid();
}
