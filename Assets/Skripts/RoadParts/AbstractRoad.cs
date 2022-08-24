using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractRoad : MonoBehaviour
{
    // Стартовая точка
    public GameObject _startPost;

    // Конечная точка
    public GameObject _endPost;

    // Ширина дороги
    public float width;

    // Количество фрагментов дороги (Детализация)
    public int details;

    // Точки, через которые проходит автомобиль
    public List<Vector3> points;

    // Угол до следующей точки. (cosA, 0, sinA)
    public List<Vector3> angles;

    // Длины сегментов дороги
    public List<float> lengthSegments;

    // Префиксные суммы длин сегментов дороги
    public List<float> prefixSumSegments;

    protected Transform _startPostTransform;
    protected Transform _endPostTransform;
    protected Vector3 startPostPosition;
    protected Vector3 endPostPosition;
    
    

    public List<GameObject> carsOnThisRoad;
    
    void Awake()
    {
        _startPostTransform = transform.Find("StartPost").transform;
        _endPostTransform = transform.Find("EndPost").transform;
        startPostPosition = _startPostTransform.position;
        endPostPosition = _endPostTransform.position;

        BuildRoad();
    }
    
    void FixedUpdate()
    {
        startPostPosition = _startPostTransform.position;
        endPostPosition = _endPostTransform.position;

        if (points[0] != startPostPosition || points[^1] != endPostPosition)
        {
            BuildRoad();
        }
    }


    protected abstract void BuildRoad();
}
