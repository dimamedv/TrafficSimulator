using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using static GlobalSettings;

public class TemplateRoad : AbstractRoad
{
    public GameObject roadPrefab;
    public float numOfLeftSideRoads = 1;
    public float numOfRightSideRoads = 1;
    public List<string> RoadsOfTemplate; 


    public override void Awake()
    {
        Initialization();
    }

    public void Start()
    {
        transform.parent = GameObject.Find("TemplateRoadFather").transform;
    }

    protected override bool NeedsRebuild()
    {
        var formingPosition = formingPoint.transform.position;
        var startPosition = startPost.transform.position;
        var endPosition = endPost.transform.position;
        return points.Count == 0
               || points[0] != startPosition
               || points[^1] != endPosition
               || !isStraight && formingPosition != _curFormingPointPosition
               || isStraight && MyMath.GetMidPoint(startPosition, endPosition) != formingPosition;
    }           
    public void Initialization()
    {
        RoadsOfTemplate = new List<string>();

        for (float i = 0.0f; i < numOfLeftSideRoads; i += 1.0f)
        {
            string roadName = "left" + i;
            RoadsOfTemplate.Add(roadName);
            CreateRoadInstance(roadName);
        } 

        for (float i = 0.0f; i < numOfRightSideRoads; i += 1.0f)
        {
            string roadName = "right" + i;
            RoadsOfTemplate.Add(roadName);
            CreateRoadInstance(roadName);
        }
    }

    private GameObject CreateRoadInstance(string roadName)
    {
        GameObject road = Instantiate(roadPrefab);

        road.name = roadName;
        road.GetComponent<SimpleRoad>().templateOwner = gameObject;
        road.transform.SetParent(gameObject.transform);

        return road;
    }

    public List<Vector3> GetBezierPointsByIdentifier(string roadIdentifier)
    {
        List<Vector3> resultPointArray = new List<Vector3>();
        points.Clear();
        if (isStraight)
        {
            formingPoint.transform.position =
                MyMath.GetMidPoint(startPost.transform.position, endPost.transform.position);
            CalculateQuadraticBezierCurve(startPost.transform.position, formingPoint.transform.position,
                endPost.transform.position, 1);
        }
        else
        {
            CalculateQuadraticBezierCurve(startPost.transform.position, formingPoint.transform.position,
            endPost.transform.position, details);
        }

        Regex directionRegex = new Regex(@"^\D+");
        Regex lineNumRegex = new Regex(@"\d+");
        string direction = directionRegex.Match(roadIdentifier).Value;
        int lineNum = int.Parse(lineNumRegex.Match(roadIdentifier).Value);

        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        if (direction == "left")
        {
            rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (direction == "right")
        {
            rotation = Quaternion.Euler(0, 90, 0);
        }

        Vector3 lineDirection = Vector3.zero; 
        for (int i = 0; i < points.Count - 1; i++)
        {
            lineDirection = (points[i + 1] - points[i]).normalized;
            resultPointArray.Add(points[i] + rotation * lineDirection * width * (lineNum + 0.5f));
        }

        resultPointArray.Add(points[^1] +  rotation * lineDirection * width * (lineNum + 0.5f));

        if (direction == "left")
        {
            resultPointArray.Reverse();
        }

        return resultPointArray;
    } 


    public override void BuildRoad(bool endIteration = true)
    {
        foreach (var road in RoadsOfTemplate)
        {
            gameObject.transform.Find(road).GetComponent<SimpleRoad>().BuildRoad(false);
        }
    }
}