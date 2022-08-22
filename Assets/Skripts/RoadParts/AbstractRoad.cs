using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractRoad : MonoBehaviour
{
    protected Transform _startPostTransform;
    protected Transform _endPostTransform;
    public Vector3 startPostPosition;
    public Vector3 endPostPosition;
    
    // Точки, через которые проходит автомобиль
    protected List<Vector3> _roadPoints;

    public float roadWidth;

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

        if (_roadPoints[0] != startPostPosition || _roadPoints[^1] != endPostPosition)
        {
            BuildRoad();
        }
    }


    protected abstract void BuildRoad();
}
