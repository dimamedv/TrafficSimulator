using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossRoadEntrance : MonoBehaviour
{
    public AbstractRoad parentRoad;
    public List<AbstractRoad> childRoads;
    public CrossRoad crossRoad;
    public bool state;

    private static List<GameObject> _entrancesList = new List<GameObject>();
    
    public void Awake()
    {
        _entrancesList.Add(gameObject);
    }

    public void OnDestroy()
    {
        _entrancesList.Remove(gameObject);
    }
    
    
}
