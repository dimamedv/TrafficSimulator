using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class TemplateRoad : AbstractRoad
{
    public GameObject roadPrefab;
    public int numOfLeftSideRoads = 1;
    public int numOfRightSideRoads = 1;
    public Dictionary<string, GameObject> RoadsOfTemplate;


    public override void Awake()
    {
        base.Awake();
        Initialization();
    }


    protected override bool NeedsRebuild()
    {
        return true;
    }

    private void Initialization()
    {
        RoadsOfTemplate = new Dictionary<string, GameObject>();

        for (int i = 0; i < numOfLeftSideRoads; i++)
        {
            string roadName = "left" + i;
            RoadsOfTemplate.Add(roadName, CreateRoadInstance(roadName));
        }

        for (int i = 0; i < numOfRightSideRoads; i++)
        {
            string roadName = "right" + i;
            RoadsOfTemplate.Add(roadName, CreateRoadInstance(roadName));
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
        CalculateQuadraticBezierCurve(startPost.transform.position, endPost.transform.position,
            formingPoint.transform.position, details);

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
            resultPointArray.Add(points[i] + rotation * lineDirection * 2 * (1 + lineNum));
        }

        resultPointArray.Add(points[^1] +  rotation * lineDirection * 2 * (1 + lineNum));

        return resultPointArray;
    }


    public override void BuildRoad(bool endIteration = true)
    {
        foreach (var road in RoadsOfTemplate)
        {
            road.Value.GetComponent<SimpleRoad>().BuildRoad(false);
        }
    }
}