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
    }

    public SimpleRoadPrototype getPrototypeFromFile(string path)
    {
        prototype = new SimpleRoadPrototype();
        string json = File.ReadAllText(path);

        prototype = JsonUtility.FromJson<SimpleRoadPrototype>(json);

        return prototype;
    }

    public void setSimpleRoadFromFile(GameObject createdRoad, string path = "Assets/Saves/testSave")
    {
        SimpleRoad roadScript = createdRoad.GetComponent<SimpleRoad>();
        SimpleRoadPrototype roadPrototype = getPrototypeFromFile(path);

        roadScript.startPost.transform.position = roadPrototype.startPostPosition;
        roadScript.endPost.transform.position = roadPrototype.endPostPosition;
        roadScript.formingPoint.transform.position = roadPrototype.formingPostPosition;
    }
    
    public void setSimpleRoadFromPrototype(GameObject createdRoad, SimpleRoadPrototype simpleRoadPrototype)
    {
        SimpleRoad roadScript = createdRoad.GetComponent<SimpleRoad>();

        roadScript.startPost.transform.position = simpleRoadPrototype.startPostPosition;
        roadScript.endPost.transform.position = simpleRoadPrototype.endPostPosition;
        roadScript.formingPoint.transform.position = simpleRoadPrototype.formingPostPosition;
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