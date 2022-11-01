using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FrameRoadsSelector : MonoBehaviour
{
    public List<CrossRoadFrame> frames;
    public List<GameObject> crossRoadEntrances;
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
        UpdateCrossRoadEntrances();
    }

    public void CloseFrame()
    {
        foreach (var road in SimpleRoad.RoadList)
        {
            DisableLineRender(road);
        }

        if (frames[currentFrame].roadsInFrameId.Count != frames[currentFrame].listOfRelations.Count)
        {
            frames[currentFrame].listOfRelations = new List<Relations>();
            foreach (var id in frames[currentFrame].roadsInFrameId)
            {
                frames[currentFrame].listOfRelations.Add(new Relations(id));
            }
        }
    }

    public void UpdateCrossRoadEntrances()
    {
        crossRoadEntrances = new List<GameObject>();
        foreach (var frame in frames)
        {
            foreach (var id in frame.roadsInFrameId)
            {
                foreach (var road in SimpleRoad.RoadList)
                {
                    if (road.GetComponent<SimpleRoad>().id == id)
                    {
                        if (!crossRoadEntrances.Contains(road.GetComponent<SimpleRoad>().parentConnection))
                        {
                            crossRoadEntrances.Add(road.GetComponent<SimpleRoad>().parentConnection);
                        }
                    }
                }
            }
        }
    }

    public bool CheckIfEntranceIsInCrossRoad(GameObject entrance)
    {
        return crossRoadEntrances.Contains(entrance);
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
        CloseFrame();
        
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

    public static void EnableLineRenderWithMaterial(GameObject road, Color color)
    {
        SimpleRoad roadScript = road.GetComponent<SimpleRoad>();
        roadScript.renderLine = true;
        roadScript.BuildRoad();
        road.GetComponent<LineRenderer>().materials[0].color = color;
        road.GetComponent<LineRenderer>().enabled = true;
    }

    public static void DisableLineRender(GameObject road)
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
        foreach (var id in roadList)
        {
            if (road.GetComponent<SimpleRoad>().id == id)
                return true;
        }

        return
            false;
    }

    // Возвращает true, если entrance является частью перекрестка
    public bool CheckIfIsEntranceToCrossRoad(GameObject checkedEntrance)
    {
        foreach (var entrance in crossRoadEntrances)
            if (checkedEntrance == entrance)
                return true;
        return false;
    }
}