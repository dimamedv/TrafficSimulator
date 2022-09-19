using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoadCreator : MonoBehaviour
{
    public LayerMask layerMask;
    public GameObject _roadPrefab;
    public int details;
    public Material material;

    private CrookedRoad crooked;
    private GameObject _road;
    private Transform _startPost;
    private Transform _endPost;
    private Transform _formingPoint;
    private int _step = 0;
    private bool _isEnable = false;
    private int _maxSteps;


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

        if (_isEnable)  
            CreateRoadSkeleton();
        else            
            DeleteObjects();
    }

    private void Update()
    {
        if (_isEnable)
        {
            UpdateObjectPosToCursorPos();
            CheckMouseButton();
        }
    }
    
    
    private void UpdateObjectPosToCursorPos()
    {
        switch (_step)
            {
                case 0:
                    MovePoint(_startPost, layerMask, true);
                    break;
                case 1:
                    crooked.enabled = true;
                    crooked.isStraight = true;
                    MovePoint(_endPost, layerMask, true);
                    break;
                case 2:
                    crooked.isStraight = false;
                    crooked.details = details;
                    MovePoint(_formingPoint, layerMask, false);
                    break;
            }
        
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

    private void CheckMouseButton()
    {
        if (Input.GetMouseButtonDown(0))
            if (_step < _maxSteps)
                _road.transform.GetChild(++_step).GetComponent<MeshRenderer>().enabled = true;
            else
            {
                for (int i = 0; i < _road.transform.childCount; i++)
                    _road.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
                CreateRoadSkeleton();
            }
        else if (Input.GetMouseButtonDown(1))
            _road.transform.GetChild(_step--).GetComponent<MeshRenderer>().enabled = false;
        else if (Input.GetMouseButtonDown(2))
            DeleteObjects();
    }

    private void CreateRoadSkeleton()
    {
        gameObject.GetComponent<RoadEditor>().enabled = false;
        _road = Instantiate(_roadPrefab);
        _road.name = "Road";
        _startPost = _road.transform.GetChild(0);
        _endPost = _road.transform.GetChild(1);
        _formingPoint = _road.transform.GetChild(2);
        for (int i = 0; i < _road.transform.childCount; i++)
            _road.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
        crooked = _road.transform.GetComponent<CrookedRoad>();
        crooked.enabled = false;
        _startPost.GetComponent<MeshRenderer>().enabled = true;
        _step = 0;
    }

    private void DeleteObjects()
    {
        gameObject.GetComponent<RoadEditor>().enabled = true;
        Destroy(_road);
        Destroy(_startPost);
        Destroy(_endPost);
        _step = 0;
        _isEnable = false;
    }
}
