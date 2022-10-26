using System;
using UnityEngine;

[Serializable]
public class SimpleRoadPrototype
{
    public Vector3 startPostPosition;
    public Vector3 endPostPosition;
    public Vector3 formingPostPosition;
    
    public int details;
    public bool isStraight;
    public bool createCrossRoadEntrance;
}