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
        points = new List<Vector3>() { _startPostTransform.position, _endPostTransform.position };
        Destroy(_roadSegment);

        GameObject createdRoadSegment = Instantiate(baseRoadSegment, transform);
        createdRoadSegment.GetComponent<RoadSegment>().startPoint = _startPostTransform.position;
        createdRoadSegment.GetComponent<RoadSegment>().endPoint = _endPostTransform.position;
        
        _roadSegment = createdRoadSegment;
    }

    protected override bool NeedsRebuild()
    {
        return points[0] != _startPostTransform.position || points[^1] != _endPostTransform.position;
    }
}