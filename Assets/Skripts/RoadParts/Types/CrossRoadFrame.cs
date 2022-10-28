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
                Relation relation = new Relation
                {
                    id = curId,
                    priority = 0
                };

                relations.Add(relation);
            }
        }
    }

    public Relation getRelationToRoadById(int neededId)
    {
        foreach (var relation in relations)
        {
            if (relation.id == neededId)
            {
                return relation;
            }
        }

        return null;
    }

    public int getPriorityToRoadById(int neededId)
    {
        Relation foundRelation = getRelationToRoadById(neededId);
        if (foundRelation != null)
        {
            return foundRelation.priority;
        }

        return 0;
    }

    public void setRelationToRoadById(int neededId, int value)
    {
        foreach (var relation in relations)
        {
            if (relation.id == neededId)
            {
                relation.priority = value;
                return;
            }
        }
    }
}

[Serializable]
public class CrossRoadFrame
{
    public List<int> idOfRoadsWithGreenLight;
    public List<Relations> listOfRelations;

    public void Initialization(List<int> newIdOfRoadsWithGreenLight)
    {
        idOfRoadsWithGreenLight = newIdOfRoadsWithGreenLight;
        
        foreach (var idOfRoad in idOfRoadsWithGreenLight)
        {
            Relations relations = new Relations();
            relations.id = idOfRoad;
            relations.Initialize(idOfRoadsWithGreenLight);
        }
    }

    public Relations getRelationsById(int neededId)
    {
        foreach (var relations in listOfRelations)
        {
            if (relations.id == neededId)
            {
                return relations;
            }
        }

        return null;
    }

    public void setRelationFromTo(int idFrom, int idTo, int value)
    {
        Relations relations = getRelationsById(idFrom);
        relations.setRelationToRoadById(idTo, value);
        
        if (value == 1)
            value = 2;
        else if (value == 2)
            value = 1;

        relations = getRelationsById(idTo);
        relations.setRelationToRoadById(idFrom, value);
    }
}