using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GlobalSettings : MonoBehaviour 
{
    public static float width = 2; 
    public static float gridStep = 1; 

    public static int targetFrameRate = 30;

    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}
