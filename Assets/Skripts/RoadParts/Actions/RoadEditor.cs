using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadEditor : MonoBehaviour
{
    public LayerMask layerMaskUI; // Слой UI
    public LayerMask layerMaskRoad; // Слой дороги
    public LayerMask layerMaskGround; // Слой земли

    private RaycastHit hitRoad;
    private GameObject objectHit; // Объект, на который упал луч hitRoad
    private GameObject lastObject = null; // Последний объект, на который упал луч hitRoad
    private Transform activePointTransform = null; // Точка, на которую упал луч hitRoad

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            bool intersectionLayerUI = Physics.Raycast(RayFromCursor.ray, 1000, layerMaskUI);
            bool intersectionLayerRoad = Physics.Raycast(RayFromCursor.ray, out hitRoad, 1000, layerMaskRoad);
            bool intersectionLayerGround = Physics.Raycast(RayFromCursor.ray, 1000, layerMaskGround);

            if (intersectionLayerUI == false && intersectionLayerRoad && activePointTransform == null)
            {
                objectHit = hitRoad.transform.gameObject;
                SimpleRoad.TurnOnKids(objectHit);

                if (objectHit.name == "Road")
                    EditRoad();
                else
                    EditPoint();
            }
            else if (intersectionLayerUI == false && intersectionLayerGround == true)
            {
                if (activePointTransform == null && lastObject != null)
                {
                    lastObject.GetComponent<MeshCollider>().enabled = true;
                    //SimpleRoad.TurnOffKids(lastObject);
                    lastObject = null;
                }
                else if (activePointTransform != null)
                {
                    activePointTransform.GetComponent<BoxCollider>().enabled = false;
                    activePointTransform = null;
                }
            }
        }

        if (activePointTransform != null)
        {
            MovePoint(activePointTransform, layerMaskGround, true);
        }
    }

    private void EditRoad()
    {
        objectHit.GetComponent<MeshCollider>().enabled = false;
        if (lastObject != null)
        {
            lastObject.GetComponent<MeshCollider>().enabled = true;
            SimpleRoad.TurnOffKids(lastObject);
        }
        lastObject = objectHit;
    }

    private void EditPoint()
    {
        objectHit.GetComponent<BoxCollider>().enabled = false;
        if (activePointTransform != null)
            activePointTransform.GetComponent<BoxCollider>().enabled = true;
        activePointTransform = objectHit.transform;
    }

    public static void MovePoint(Transform _transform, LayerMask _layerMask, bool _rebuildPointByGrid)
    {
        RaycastHit hit;
        if (Physics.Raycast(RayFromCursor.ray, out hit, 1000, _layerMask))
        {
            _transform.position = hit.point;
            if (_rebuildPointByGrid)
                AbstractRoad.RebuildPointByGrid(_transform);
        }
    }  
}