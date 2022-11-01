using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Relations
{
    public int id;
    public List<int> roadsToTrackId;

    public Relations(int id)
    {
        this.id = id;
        roadsToTrackId = new List<int>();
    }
}


[Serializable]
public class CrossRoadFrame 
{
    public List<Relations> listOfRelations;
    public List<int> roadsInFrameId;
    public float time = 20.0f;

    public CrossRoadFrame()
    {
        roadsInFrameId = new List<int>();
    }

    public void Initialize(List<int> roadsWithGreenLightId)
    {
        listOfRelations = new List<Relations>();
        roadsInFrameId = roadsWithGreenLightId;
        
        for (int i = 0; i < roadsWithGreenLightId.Count; i++)
        {
            Relations relations = new Relations(roadsWithGreenLightId[i]);
            listOfRelations.Add(relations);
        }
    }

    public List<int> GetRoadToTrackById(int id)
    {
        foreach (var relations in listOfRelations)
        {
            if (relations.id == id)
                return relations.roadsToTrackId;
        }

        return null;
    }

    public void SetRoadsToTrackById(int id, List<int> roadToTrackIds)
    {
        foreach (var relations in listOfRelations)
        {
            if (relations.id == id)
            {
                relations.roadsToTrackId = roadToTrackIds;
            }
        }
    }
    
    public void AddRoadsToTrackById(int id, int roadToAdd)
    {
        foreach (var relations in listOfRelations)
        {
            if (relations.id == id)
            {
                relations.roadsToTrackId.Add(roadToAdd);
            }
        }
    }

    public GameObject GetRoadById(int id)
    {
        foreach (var road in SimpleRoad.RoadList)
        {
            if (road.GetComponent<SimpleRoad>().id == id)
                return road;
        }

        return null;
    }
}