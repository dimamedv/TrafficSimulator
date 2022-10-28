using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum State
{
    Green,
    Yell,
    Red
}

[Serializable]
public class Relation
{
    public int id;
    public int priority;
}


[Serializable]
public class Relations
{
    public int id;
    public List<Relation> relations;

    public void Initialize(List<int> idOfRoadWithGreenLight)
    {
        relations = new List<Relation>();
        foreach (var curId in idOfRoadWithGreenLight) 
        {
            if (curId != id)
            {
                Relation relation = new Relation();
                relation.id = curId;
                relation.priority = 0;
                
                relations.Add(relation);
            }
        }
    }
}

[Serializable]
public class CrossRoadFrame
{
    public List<int> idOfRoadsWithGreenLight;
    public List<Relations> listOfRelations;

    
}