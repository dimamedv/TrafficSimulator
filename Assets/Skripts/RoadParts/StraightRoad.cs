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
        points = new List<Vector3>() { _startPost.transform.position, _endPost.transform.position };
        Destroy(_roadSegment);

        GameObject createdRoadSegment = Instantiate(baseRoadSegment, transform);
        createdRoadSegment.GetComponent<RoadSegment>().startPoint = _startPost.transform.position;
        createdRoadSegment.GetComponent<RoadSegment>().endPoint = _endPost.transform.position;
        
        _roadSegment = createdRoadSegment;
    }
    protected override void RebuildGrid()
    {
        RebuildGridByPoint(ref _startPost);
        RebuildGridByPoint(ref _endPost);
    }

    protected override bool isNeedsRebuild()
    {
        return points[0] != _startPost.transform.position || points[^1] != _endPost.transform.position;
    }
}