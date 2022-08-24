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
    protected Vector3 curStartPosition;
    protected Vector3 curEndPosition;
    
    

    public List<GameObject> carsOnThisRoad;
    
    public void Awake()
    {
        _startPostTransform = transform.Find("StartPost").transform;
        _endPostTransform = transform.Find("EndPost").transform;
        curStartPosition = _startPostTransform.position;
        curEndPosition = _endPostTransform.position;

        BuildRoad();
    }

    void FixedUpdate()
    {
        if (points[0] != _startPostTransform.position || points[^1] != _endPostTransform.position)
        {
            BuildRoad();
        }
    }


    protected abstract void BuildRoad();
    protected abstract bool NeedsRebuild();
}
