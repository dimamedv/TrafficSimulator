using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class StraightRoad : AbstractRoad
{
    public GameObject baseRoadSegment;
    private GameObject _roadSegment;

    protected override void BuildRoad()
    {
        _roadPoints = new List<Vector3>() { startPostPosition, endPostPosition };
        Destroy(_roadSegment);

        GameObject createdRoadSegment = Instantiate(baseRoadSegment, transform);
        createdRoadSegment.GetComponent<RoadSegment>().startPoint = startPostPosition;
        createdRoadSegment.GetComponent<RoadSegment>().endPoint = endPostPosition;
        
        _roadSegment = createdRoadSegment;
    }
}