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

    public LayerMask layerMaskUI; // ���� UI
    public LayerMask layerMaskRoad; // ���� ������


    private void Awake()
    {
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
                    if (!secondarySelectedRoadsId.Contains(hitRoad.GetComponent<SimpleRoad>().id))
                    {
                        secondarySelectedRoadsId.Add(hitRoad.GetComponent<SimpleRoad>().id);
                        EnableLineRenderWithMaterial(hitRoad, Color.red);
                    }
                    else
                    {
                        secondarySelectedRoadsId.Remove(hitRoad.GetComponent<SimpleRoad>().id);
                        DisableLineRender(hitRoad);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            foreach (var road in SimpleRoad.RoadList)
            {
                if (secondarySelectedRoadsId.Contains(road.GetComponent<SimpleRoad>().id))
                {
                    DisableLineRender(road);
                }
            }

            frames[currentFrame]
                .SetRoadsToTrackById(selectedRoad.GetComponent<SimpleRoad>().id, secondarySelectedRoadsId);

            DisableLineRender(selectedRoad);
            selectedRoad = null;
        }
    }

    public void EnableLineRenderWithMaterial(GameObject road, Color color)
    {
        SimpleRoad roadScript = road.GetComponent<SimpleRoad>();
        roadScript.renderLine = true;
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