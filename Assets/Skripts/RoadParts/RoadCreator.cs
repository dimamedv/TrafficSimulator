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
    public int details;

    private CrookedRoad crooked;
    Vector3 epsV = new Vector3(0f, 0.001f, 0f);
    GameObject _road;
    GameObject _startPost;
    GameObject _endPost;
    GameObject _formingPoint;
    int _step;
    int _maxSteps;
    bool _isEnable;


    private void Awake()
    {
        _isEnable = false;
        _step = 0;
    }

    public void ButtonStraightIsPressed()
    {
        _maxSteps = 1;
        ButtonIsPressed();
    }

    public void ButtonCrookedIsPressed()
    {
        _maxSteps = 2;
        ButtonIsPressed();
    }

    public void ButtonIsPressed()
    {
        _isEnable = !_isEnable;
        if (_isEnable) CreateObjects();
        else DeleteObjects();
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
                    AbstractRoad.RebuildGridByPoint(ref _startPost);
                    break;
                case 1:
                    crooked.enabled = true;
                    crooked.isStraight = true;
                    _endPost.transform.position = hit.point;
                    break;
                case 2:
                    crooked.isStraight = false;
                    crooked.details = details;
                    _formingPoint.transform.position = hit.point;
                    break;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (_step < _maxSteps)
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
        _road.transform.position += epsV;
        _startPost = CreateObject(ref _startPost, cubeRed, "StartPost", true);
        _endPost = CreateObject(ref _endPost, cubeGreen, "EndPost", false);
        _formingPoint = CreateObject(ref _formingPoint, cubeBlue, "FormingPoint", false);
        crooked = _road.AddComponent<CrookedRoad>();
        crooked.enabled = false;
        _road.AddComponent<MeshFilter>();
        _road.AddComponent<MeshRenderer>();
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
