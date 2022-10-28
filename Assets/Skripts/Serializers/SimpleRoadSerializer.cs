using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class SimpleRoadSerializer
{
    public SimpleRoadPrototype prototype;


    private void fillPrototype(GameObject simpleRoad)
    {
        prototype = new SimpleRoadPrototype();
        SimpleRoad simpleRoadScript = simpleRoad.GetComponent<SimpleRoad>();

        prototype.startPostPosition = simpleRoadScript.startPost.transform.position;
        prototype.endPostPosition = simpleRoadScript.endPost.transform.position;
        prototype.formingPostPosition = simpleRoadScript.formingPoint.transform.position;

        prototype.details = simpleRoadScript.details;
        prototype.isStraight = simpleRoadScript.isStraight;
        prototype.createCrossRoadEntrance = simpleRoadScript.createCrossRoadEntrance;

        prototype.id = simpleRoadScript.id;
    }

    public SimpleRoadPrototype getPrototypeFromFile(string path)
    {
        prototype = new SimpleRoadPrototype();
        string json = File.ReadAllText(path);

        prototype = JsonUtility.FromJson<SimpleRoadPrototype>(json);

        return prototype;
    }

    public void setSimpleRoadFromPrototype(GameObject createdRoad, SimpleRoadPrototype simpleRoadPrototype)
    {
        SimpleRoad roadScript = createdRoad.GetComponent<SimpleRoad>();

        roadScript.startPost.transform.position = simpleRoadPrototype.startPostPosition;
        roadScript.endPost.transform.position = simpleRoadPrototype.endPostPosition;
        roadScript.formingPoint.transform.position = simpleRoadPrototype.formingPostPosition;

        roadScript.details = simpleRoadPrototype.details;
        roadScript.isStraight = simpleRoadPrototype.isStraight;
        roadScript.createCrossRoadEntrance = simpleRoadPrototype.createCrossRoadEntrance;

        roadScript.id = simpleRoadPrototype.id;
    }

    public List<SimpleRoadPrototype> getListOfAllSimpleRoadPrototypes()
    {
        List<SimpleRoadPrototype> listOfSimpleRoadPrototypes = new List<SimpleRoadPrototype>();
        GameObject roadFather = GameObject.Find("RoadFather");
        
        for (int i = 0; i < roadFather.transform.childCount; i++)
        {
            fillPrototype(roadFather.transform.GetChild(i).gameObject);
            listOfSimpleRoadPrototypes.Add(prototype);
        }

        return listOfSimpleRoadPrototypes;
    }
}