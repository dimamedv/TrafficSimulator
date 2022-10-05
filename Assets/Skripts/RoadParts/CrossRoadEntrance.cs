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

    private void Start()
    {
        
    }
}
