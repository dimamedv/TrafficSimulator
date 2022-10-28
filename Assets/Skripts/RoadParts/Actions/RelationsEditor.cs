using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RelationsEditor : MonoBehaviour
{
    public GameObject selectedRoad;
    public List<int> chosenRoadsId;
    public List<int> secondarySelectedRoadsId;
    public List<CrossRoadFrame> frames;
    public int currentFrame;

    public LayerMask layerMaskUI; // Слой UI
    public LayerMask layerMaskRoad; // Слой дороги


    private void Awake()
    {
        frames = new List<CrossRoadFrame>();
        frames.Add(new CrossRoadFrame());
        frames[0].Initialize(new List<int> {0, 2, 3});
        chosenRoadsId = new List<int> { 0, 2, 3 };
        currentFrame = 0;
        
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject hitRoad = RayCastRoad();
            if (hitRoad != null && chosenRoadsId.Contains(hitRoad.GetComponent<SimpleRoad>().id))
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
                        DisableLineRender(hitRoad);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(1) && selectedRoad)
        {
            foreach (var road in SimpleRoad.RoadList)
            {
                if (secondarySelectedRoadsId.Contains(road.GetComponent<SimpleRoad>().id))
                {
                    DisableLineRender(road);
                }
            }
            
            DisableLineRender(selectedRoad);
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
        roadScript.renderLine = true;
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
}