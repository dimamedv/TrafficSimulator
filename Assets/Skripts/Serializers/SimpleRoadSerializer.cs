using System.IO;
using UnityEngine;


public class SimpleRoadSerializer
{
    public SimpleRoadPrototype prototype;
    
    public void serializeSimpleRoad(GameObject simpleRoad)
    {
        prototype = new SimpleRoadPrototype();
        SimpleRoad simpleRoadScript = simpleRoad.GetComponent<SimpleRoad>();

        prototype.startPostPosition = simpleRoadScript.startPost.transform.position;
        prototype.endPostPosition = simpleRoadScript.endPost.transform.position;
        prototype.formingPostPosition = simpleRoadScript.formingPoint.transform.position;
        
        string jsonView = JsonUtility.ToJson(prototype);
        File.WriteAllText("Assets/Saves/testSave", jsonView);
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
}