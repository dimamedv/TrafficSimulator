using System;
using UnityEngine;

[Serializable]
public class TemplateRoadPrototype
{
    public Vector3 startPostPosition;
    public Vector3 endPostPosition;
    public Vector3 formingPostPosition;
    
    public bool isStraight;

    public int numOfLeftSideRoads = 1;
    public int numOfRightSideRoads = 1;
}