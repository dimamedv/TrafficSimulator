using System;
using UnityEngine;

[Serializable]
public class TemplateRoadPrototype
{
    public Vector3 startPostPosition;
    public Vector3 endPostPosition;
    public Vector3 formingPostPosition;
    
    public bool isStraight;

    public float numOfLeftSideRoads = 1;
    public float numOfRightSideRoads = 1;
}