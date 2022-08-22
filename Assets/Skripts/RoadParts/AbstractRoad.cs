using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractRoad : MonoBehaviour
{
    public RoadCharacteristics charact;
    protected Transform _startPostTransform;
    protected Transform _endPostTransform;
    protected Vector3 startPostPosition;
    protected Vector3 endPostPosition;
    
    

    public List<GameObject> carsOnThisRoad;
    
    void Awake()
    {
        charact = GetComponent<RoadCharacteristics>();
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

        if (charact.points[0] != startPostPosition || charact.points[^1] != endPostPosition)
        {
            BuildRoad();
        }
    }


    protected abstract void BuildRoad();
}
