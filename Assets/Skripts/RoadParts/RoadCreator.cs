using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoadCreator : MonoBehaviour
{
    public LayerMask layerMaskGround;
    public GameObject _roadPrefab;
    public int details;
    public Material material;

    private SimpleRoad s1mple; // Бро, надо тренироваться
    private GameObject _road;
    private Transform _startPost;
    private Transform _endPost;
    private Transform _formingPoint;
    private int _step = 0;
    private bool _isEnable = false;
    private int _maxSteps = 1;


    public void ButtonStraightIsPressed()
    {
        _maxSteps = 1;
    }

    public void ButtonCrookedIsPressed()
    {
        _maxSteps = 2;
    }

    public void ButtonCreateRoad()
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
                    RoadEditor.MovePoint(_startPost, layerMaskGround, true);
                    break;
                case 1:
                    s1mple.enabled = true;
                    s1mple.isStraight = true;
                    RoadEditor.MovePoint(_endPost, layerMaskGround, true);
                    break;
                case 2:
                    s1mple.isStraight = false;
                    s1mple.details = details;
                    RoadEditor.MovePoint(_formingPoint, layerMaskGround, false);
                    break;
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
        s1mple = _road.transform.GetComponent<SimpleRoad>();
        s1mple.enabled = false;
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
