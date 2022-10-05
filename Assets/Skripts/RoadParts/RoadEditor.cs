using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadEditor : MonoBehaviour
{
    public LayerMask layerMaskRoad;
    public LayerMask layerMaskGround;

    private GameObject lastObject = null;
    private GameObject objectHit;
    private Transform activePointTransform = null;
    private RaycastHit hit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (activePointTransform == null && Physics.Raycast(RayFromCursor.ray, out hit, 1000, layerMaskRoad))
            {
                objectHit = hit.transform.gameObject;
                SimpleRoad.TurnOnKids(objectHit);

                if (objectHit.name == "Road")
                {
                    objectHit.GetComponent<MeshCollider>().enabled = false;
                    if (lastObject != null)
                    {
                        lastObject.GetComponent<MeshCollider>().enabled = true;
                        SimpleRoad.TurnOffKids(lastObject);
                    }
                    lastObject = objectHit;
                }
                else
                {
                    objectHit.GetComponent<BoxCollider>().enabled = false;
                    if (activePointTransform != null)
                        activePointTransform.GetComponent<BoxCollider>().enabled = true;
                    activePointTransform = objectHit.transform;
                }
            }
            else if (Physics.Raycast(RayFromCursor.ray, 1000, layerMaskGround))
            {
                if (activePointTransform == null)
                {
                    lastObject.GetComponent<MeshCollider>().enabled = true;
                    SimpleRoad.TurnOffKids(lastObject);
                    lastObject = null;
                }
                else
                {
                    activePointTransform.GetComponent<BoxCollider>().enabled = false;
                    activePointTransform = null;
                }
            }
        }

        if (Physics.Raycast(RayFromCursor.ray, out hit, 1000, layerMaskGround) && activePointTransform != null)
        {
            RoadCreator.MovePoint(activePointTransform, layerMaskGround, true);
        }
    }
}