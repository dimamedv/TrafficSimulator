using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RelationsEditor : MonoBehaviour
{
    public GameObject selectedRoad;
    public List<int> secondarySelectedRoadsId;
    public List<CrossRoadFrame> frames;
    public int currentFrame;

    public LayerMask layerMaskUI; // Слой UI
    public LayerMask layerMaskRoad; // Слой дороги


    private void OnEnable()
    {
        frames = gameObject.GetComponent<FrameRoadsSelector>().frames;
        OpenFrame(currentFrame);
    }

    private void OnDisable()
    {
        CloseFrame();
    }

    public void OpenFrame(int frameId)
    {
        currentFrame = frameId;
        selectedRoad = null;

        foreach (var road in SimpleRoad.RoadList)
        {
            if (CheckIfRoadInListById(road, frames[frameId].roadsInFrameId))
            {
                EnableLineRenderWithMaterial(road, Color.blue);
            }
        }
    }

    public void CloseFrame()
    {
        foreach (var road in SimpleRoad.RoadList)
        {
            DisableLineRender(road);
        }
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject hitRoad = RayCastRoad();
            if (hitRoad != null && CheckIfRoadInListById(hitRoad, frames[currentFrame].roadsInFrameId))
            {
                if (selectedRoad == null)
                {
                    selectedRoad = hitRoad;
                    EnableLineRenderWithMaterial(selectedRoad, Color.green);
                    secondarySelectedRoadsId = frames[currentFrame]
                        .GetRoadToTrackById(selectedRoad.GetComponent<SimpleRoad>().id);

                    foreach (var road in SimpleRoad.RoadList)
                    {
                        if (secondarySelectedRoadsId.Contains(road.GetComponent<SimpleRoad>().id))
                        {
                            EnableLineRenderWithMaterial(road, Color.red);
                        }
                    }
                }
                else
                {
                    if (!secondarySelectedRoadsId.Contains(hitRoad.GetComponent<SimpleRoad>().id) &&
                        hitRoad.GetComponent<SimpleRoad>().id != selectedRoad.GetComponent<SimpleRoad>().id)
                    {
                        secondarySelectedRoadsId.Add(hitRoad.GetComponent<SimpleRoad>().id);
                        EnableLineRenderWithMaterial(hitRoad, Color.red);
                    }
                    else if (hitRoad.GetComponent<SimpleRoad>().id != selectedRoad.GetComponent<SimpleRoad>().id)
                    {
                        secondarySelectedRoadsId.Remove(hitRoad.GetComponent<SimpleRoad>().id);
                        EnableLineRenderWithMaterial(hitRoad, Color.blue);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(1) && selectedRoad)
        {
            foreach (var road in SimpleRoad.RoadList)
            {
                if (frames[currentFrame].roadsInFrameId.Contains(road.GetComponent<SimpleRoad>().id))
                {
                    EnableLineRenderWithMaterial(road, Color.blue);
                }
            }

            selectedRoad = null;
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