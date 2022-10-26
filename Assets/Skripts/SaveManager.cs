using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public GameObject simpleRoadPrefab;
    private void Awake()
    {
        SimpleRoadSerializer simpleRoadSerializer = new SimpleRoadSerializer();
        GameObject createdRoad = Instantiate(simpleRoadPrefab);
        createdRoad.name = "roadFromSave";
        
        simpleRoadSerializer.setSimpleRoadFromFile(createdRoad);
    }

    public static void WritePrototypeInFile(object prototype, string path)
    {
        string jsonView = JsonUtility.ToJson(prototype);
        File.WriteAllText("Assets/Saves/testSave", jsonView);
    }
}
