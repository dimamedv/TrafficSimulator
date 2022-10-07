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
            if (activePointTransform == null && Physics.Raycast(RayFromCursor.ray, out hit, 1000, layerMaskRoad))
                StartEditRoad();
            else if (Physics.Raycast(RayFromCursor.ray, 1000, layerMaskGround))
                StopEditRoad();

        if (Physics.Raycast(RayFromCursor.ray, out hit, 1000, layerMaskGround) && activePointTransform != null)
            RoadCreator.MovePoint(activePointTransform, layerMaskGround, true);
    }

    private void StartEditRoad()
    {
        objectHit = hit.transform.gameObject;
        TemplateRoad.TurnOnPoints(objectHit);

        if (objectHit.name == "Road")
        {
            objectHit.GetComponent<MeshCollider>().enabled = false;
            if (lastObject != null)
            {
                lastObject.GetComponent<MeshCollider>().enabled = true;
                TemplateRoad.TurnOffPoints(lastObject);
                Debug.Log("0");
            }
            Debug.Log("1");
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

    private void StopEditRoad()
    {
        if (activePointTransform == null)
        {
            lastObject.GetComponent<MeshCollider>().enabled = true;
            TemplateRoad.TurnOffPoints(lastObject);
            lastObject = null;
        }
        else
        {
            activePointTransform.GetComponent<BoxCollider>().enabled = true;
            activePointTransform = null;
        }
    }
}
