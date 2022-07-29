using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class RoadBehaviour : MonoBehaviour
{
    private Transform _startPostTransform;
    private Transform _endPostTransform;

    public bool isStraightRoad = true;
    public GameObject baseRoadSegment;
    private GameObject[] _roadSegments;
    private Vector3[] _roadPoints;

    public Vector3 startPostPosition;
    public Vector3 endPostPosition;


    public Vector3 CalculateMidPoint(Vector3 v1, Vector3 v2)
    {
        return v1 + (v2 - v1) / 2;
    }

    private void BuildRoad()
    {
        if (isStraightRoad)
        {
            _roadPoints = new[] {startPostPosition, endPostPosition}; 
            
            GameObject createdRoadSegment = Instantiate(baseRoadSegment, transform);
            createdRoadSegment.GetComponent<RoadSegment>().startPoint = startPostPosition;
            createdRoadSegment.GetComponent<RoadSegment>().endPoint = endPostPosition;

            _roadSegments = new[] { createdRoadSegment };
        }
        else
        {
            
        }
    }

    private void Awake()
    {
        _startPostTransform = transform.Find("StartPost").transform;
        _endPostTransform = transform.Find("EndPost").transform;
        startPostPosition = _startPostTransform.position;
        endPostPosition = _endPostTransform.position;
        
        BuildRoad();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        startPostPosition = _startPostTransform.position;
        endPostPosition = _endPostTransform.position;

        if (_roadPoints[0] != startPostPosition || _roadPoints[^1] != endPostPosition)
        {
            for(int i = 0; i < _roadSegments.Length; i++) 
            {
                Destroy(_roadSegments[i]);
            }
            BuildRoad();
        }

    }
}
