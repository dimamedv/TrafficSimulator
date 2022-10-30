using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRoadsSelector : MonoBehaviour
{
    public List<CrossRoadFrame> frames;
    public int currentFrame;

    public LayerMask layerMaskUI; // Слой UI
    public LayerMask layerMaskRoad; // Слой дороги

    public void Awake()
    {
        frames = new List<CrossRoadFrame>();
        currentFrame = 0;

        UpdateFrameListByNumber(1);
        OpenFrame(currentFrame);
    }

    public void OnEnable()
    {
        OpenFrame(currentFrame);
    }

    public void OnDisable()
    {
        CloseFrame();
    }

    public void CloseFrame()
    {
        foreach (var road in SimpleRoad.RoadList)
        {
            DisableLineRender(road);
        }
    }

    public void IncreaseFrameCount()
    {
        UpdateFrameListByNumber(frames.Count + 1);
    }
    
    public void DecreaseFrameCount()
    {
        if (frames.Count - 1 >= 0)
            UpdateFrameListByNumber(frames.Count - 1);
    }

    public void OpenFrame(int frameIndex)
    {
        if (frameIndex < frames.Count)
            currentFrame = frameIndex;

        foreach (var road in SimpleRoad.RoadList)
        {
            if (road.GetComponent<SimpleRoad>().templateOwner == null)
            {
                EnableLineRenderWithMaterial(road, Color.blue);
            }

            if (CheckIfRoadInListById(road, frames[frameIndex].roadsInFrameId))
            {
                EnableLineRenderWithMaterial(road, Color.green);
            }
            
        }
        
        
    }

    public void UpdateFrameListByNumber(int number)
    {
        while (frames.Count > number)
        {
            frames.RemoveAt(frames.Count - 1);
        }

        while (frames.Count < number)
        {
            frames.Add(new CrossRoadFrame());
            
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject hitRoad = RayCastRoad();

            if (hitRoad)
            {
                if (CheckIfRoadInListById(hitRoad, frames[currentFrame].roadsInFrameId))
                {
                    frames[currentFrame].roadsInFrameId.Remove(hitRoad.GetComponent<SimpleRoad>().id);
                    EnableLineRenderWithMaterial(hitRoad, Color.blue);
                }
                else
                {
                    frames[currentFrame].roadsInFrameId.Add(hitRoad.GetComponent<SimpleRoad>().id);
                    EnableLineRenderWithMaterial(hitRoad, Color.green);
                }
            }
        }
    }

    public void EnableLineRenderWithMaterial(GameObject road, Color color)
    {
        SimpleRoad roadScript = road.GetComponent<SimpleRoad>();
        roadScript.renderLine = true;
        roadScript.BuildRoad();
        road.GetComponent<LineRenderer>().materials[0].color = color;
        road.GetComponent<LineRenderer>().enabled = true;
    }

    public void DisableLineRender(GameObject road)
    {
        SimpleRoad roadScript = road.GetComponent<SimpleRoad>();
        roadScript.renderLine = false;
        road.GetComponent<LineRenderer>().enabled = false;
    }

    public GameObject RayCastRoad()
    {
        RaycastHit hit;

        bool intersectionLayerUI = Physics.Raycast(RayFromCursor.ray, 1000, layerMaskUI);
        bool intersectionLayerRoad = Physics.Raycast(RayFromCursor.ray, out hit, 1000, layerMaskRoad);

        if (!intersectionLayerUI && intersectionLayerRoad && hit.transform.name == "Road")
        {
            return hit.transform.gameObject;
        }

        return null;
    }


    public bool CheckIfRoadInListById(GameObject road, List<int> roadList)
    {
        return roadList.Contains(road.GetComponent<SimpleRoad>().id);
    }
}