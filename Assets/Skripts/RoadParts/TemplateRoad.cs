using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateRoad : AbstractRoad
{
    public GameObject roadPrefab;
    public int numOfLeftSideRoads = 1;
    public int numOfRightSideRoads = 1;
    public Dictionary<string, GameObject> RoadsOfTemplate;


    public void Awake()
    {
        Initialization();
    }


    protected override bool NeedsRebuild()
    {
        return false;
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

    public List<Vector3> GetBezierPointsByIdentifier()
    {

        return null;
    }
    

    protected override void BuildRoad(bool endIteration = true)
    {
        
    }
}
