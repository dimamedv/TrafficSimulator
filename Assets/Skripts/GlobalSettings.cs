using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GlobalSettings : MonoBehaviour 
{
    public static float width = 3; 
    public static float gridStep = 1;
    public static float SaveDistance = 5;

    public int nextRoadNumeration;

    public static int targetFrameRate = 30;

    private void Awake()
    {
        nextRoadNumeration = 0;
    }

    public int GetNextRoadNumeration()
    {
        return nextRoadNumeration++;
    }

    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}
