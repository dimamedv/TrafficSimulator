using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public GameObject simpleRoadPrefab;
    public GameObject templateRoadPrefab;
    private void Awake()
    {
        
    }

    public static void WritePrototypeInFile(object prototype, string path)
    {
        string jsonView = JsonUtility.ToJson(prototype);
        File.WriteAllText(path, jsonView);
    }

    public void MakeSave(string path = "Assets/Saves/Save")
    {
        SaveFile save = new SaveFile();

        SimpleRoadSerializer simpleRoadSerializer = new SimpleRoadSerializer();
        save.listOfSimpleRoadPrototypes = simpleRoadSerializer.getListOfAllSimpleRoadPrototypes();

        TemplateRoadSerializer templateRoadSerializer = new TemplateRoadSerializer();
        save.listOfTemplateRoadPrototypes = templateRoadSerializer.getListOfAllSimpleRoadPrototypes();
        
        WritePrototypeInFile(save, path);
        Debug.Log("GameSaved in " + path);
    }

    public void ResetGameFromSave(string path)
    {
        string json = File.ReadAllText(path);
        SaveFile saveFile = JsonUtility.FromJson<SaveFile>(json);
        
        for (int i = 0; i < saveFile.listOfTemplateRoadPrototypes.Count; i++)
        {
            TemplateRoadSerializer templateRoadSerializer = new TemplateRoadSerializer();
            GameObject createdRoad = Instantiate(templateRoadPrefab);

            templateRoadSerializer.setTemplateRoadFromPrototype(createdRoad, saveFile.listOfTemplateRoadPrototypes[i]);
        }

        for (int i = 0; i < saveFile.listOfSimpleRoadPrototypes.Count; i++)
        {
            SimpleRoadSerializer simpleRoadSerializer = new SimpleRoadSerializer();
            GameObject createdRoad = Instantiate(simpleRoadPrefab);
            createdRoad.name = "Road";

            simpleRoadSerializer.setSimpleRoadFromPrototype(createdRoad, saveFile.listOfSimpleRoadPrototypes[i]);
        }
        
    }
}
