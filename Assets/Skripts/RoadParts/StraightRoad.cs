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
        points = new List<Vector3>() { startPost.transform.position, endPost.transform.position };
        Destroy(_roadSegment);

        GameObject createdRoadSegment = Instantiate(baseRoadSegment, transform);
        createdRoadSegment.GetComponent<RoadSegment>().startPoint = startPost.transform.position;
        createdRoadSegment.GetComponent<RoadSegment>().endPoint = endPost.transform.position;
        
        _roadSegment = createdRoadSegment;
    }

    protected override bool NeedsRebuild()
    {
        return points[0] != startPost.transform.position || points[^1] != endPost.transform.position;
    }
}