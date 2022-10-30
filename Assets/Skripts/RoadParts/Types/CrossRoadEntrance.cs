using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossRoadEntrance : MonoBehaviour
{
    public List<GameObject> parentRoads;
    public List<GameObject> childRoads;
    public CrossRoad crossRoad;
    public bool state;

    public static List<GameObject> EntrancesList = new List<GameObject>();
    
    public void Awake()
    {
        EntrancesList.Add(gameObject);
    }

    public void OnDestroy()
    {
        EntrancesList.Remove(gameObject);
    }
}
