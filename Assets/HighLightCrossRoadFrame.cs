using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FrameRoadsSelector;

public class HighLightCrossRoadFrame : MonoBehaviour
{
    public GameObject roadFather;

    private void OnEnable()
    {
        HighLightFrame();
    }

    private void OnDisable()
    {
        turnOffHighLightOfFrame();
    }

    public void turnOffHighLightOfFrame()
    {
        foreach (var road in SimpleRoad.RoadList)
        {
            DisableLineRender(road);
        }
    }

    public void HighLightFrame()
    {
        roadFather = GameObject.Find("RoadFather");
        turnOffHighLightOfFrame();

        int frameIndex = roadFather.GetComponent<CrossRoadManager>().currentFrameIndex;
        CrossRoadFrame frameToHighLight = roadFather.GetComponent<FrameRoadsSelector>().frames[frameIndex];
        
        foreach (var road in SimpleRoad.RoadList)
        {
            if (road.GetComponent<SimpleRoad>().templateOwner == null)
            {
                EnableLineRenderWithMaterial(road, Color.yellow);
            }

            return;
        }
        
        foreach (var road in SimpleRoad.RoadList)
        {
            FrameRoadsSelector selector = roadFather.GetComponent<FrameRoadsSelector>();

            if (road.GetComponent<SimpleRoad>().templateOwner == null)
            {
                if (selector.CheckIfRoadInListById(road, frameToHighLight.roadsInFrameId))
                    EnableLineRenderWithMaterial(road, Color.green);
                else
                    EnableLineRenderWithMaterial(road, Color.red);
            }
        }
    }

    public GameObject GetRoadById(int id)
    {
        foreach (var road in SimpleRoad.RoadList)
        {
            if (road.GetComponent<SimpleRoad>().id == id)
                return road;
        }

        return null;
    }


    // Update is called once per frame
    void Update()
    {
    }
}