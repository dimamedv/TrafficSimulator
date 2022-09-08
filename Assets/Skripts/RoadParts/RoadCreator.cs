using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoadCreator : MonoBehaviour
{
    public LayerMask layerMask;
    public bool isStraight;
    public GameObject cubeRed;
    public GameObject cubeGreen;
    public GameObject cubeBlue;

    GameObject _road;
    GameObject _startPost;
    GameObject _endPost;
    GameObject _formingPoint;
    int _step;
    bool _isEnable;


    private void Awake()
    {
        _isEnable = false;
        _step = 0;
    }

    public void ButtonIsPressed()
    {
        _isEnable = !_isEnable;
        if (_isEnable)
        {
            CreateObjects();
        }
        else
        {
            DeleteObjects();
        }
    }

    void Update()
    {
        if (!_isEnable) return; 

        RaycastHit hit;
        if (Physics.Raycast(RayFromCursor.ray, out hit, 1000, layerMask))
        {
            switch (_step)
            {
                case 0:
                    _startPost.transform.position = hit.point;
                    break;
                case 1:
                    _endPost.transform.position = hit.point;
                    _road.AddComponent<CrookedRoad>();
                    CrookedRoad ce = _road.GetComponent<CrookedRoad>();
                    ce.isStraight = true;
                    ce.debugRoad = true;
                    break;
                case 2:
                    _formingPoint.transform.position = hit.point;
                    break;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (_step < 2)
            _road.transform.GetChild(++_step).GetComponent<MeshRenderer>().enabled = true;
            else
                CreateObjects();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            _road.transform.GetChild(_step--).GetComponent<MeshRenderer>().enabled = false;
        }
        else if (Input.GetMouseButtonDown(2))
        {
            DeleteObjects();
        }
    }

    private void CreateObjects()
    {
        _road = new GameObject("CrookedRoad");
        _startPost = CreateObject(ref _startPost, cubeRed, "StartPost", true);
        _endPost = CreateObject(ref _endPost, cubeGreen, "EndPost", false);
        _formingPoint = CreateObject(ref _formingPoint, cubeBlue, "FormingPoint", false);
        _step = 0;
    }

    private ref GameObject CreateObject(ref GameObject __gameObject, GameObject __prefab, string __name, bool __isVisible = false)
    {
        __gameObject = Instantiate(__prefab);
        __gameObject.transform.SetParent(_road.transform, false);
        __gameObject.name = __name;
        __gameObject.GetComponent<MeshRenderer>().enabled = __isVisible;
        return ref __gameObject;
    }

    private void DeleteObjects()
    {
        Destroy(_road);
        Destroy(_startPost);
        Destroy(_endPost);
        _step = 0;
        _isEnable = false;
    }
}
