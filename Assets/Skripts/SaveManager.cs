using System;
using System.Collections;
using System.Collections.Generic;
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
}
