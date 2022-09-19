using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadEditor : MonoBehaviour
{
    public LayerMask layerMaskRoad;
    public LayerMask layerMaskGround;

    public GameObject lastObject = null;
    public GameObject objectHit;
    public Transform activePointTransform = null;
    public RaycastHit hit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(RayFromCursor.ray, out hit, 1000, layerMaskRoad))
            {
                objectHit = hit.transform.gameObject;
                AbstractRoad.TurnOnKids(objectHit);

                if (objectHit.name == "Road")
                {
                    objectHit.GetComponent<MeshCollider>().enabled = false;
                    if (lastObject != null)
                        lastObject.GetComponent<MeshCollider>().enabled = true;
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
                    AbstractRoad.TurnOffKids(lastObject);
                    lastObject = null;
                }
                else
                {
                    activePointTransform.GetComponent<BoxCollider>().enabled = true;
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
