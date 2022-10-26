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

    private TemplateRoad template; // Бро, надо тренироваться
    private GameObject _road;
    private Transform _startPost;
    private Transform _endPost;
    private Transform _formingPoint;
    private int _step = 0;
    private bool _isEnable = false;
    private bool _renderMesh = true;
    private int _maxSteps = 1;
    private float _rightLanes = 1;
    private float _leftLanes = 1;

    public void ButtonStraightIsPressed()
    {
        _maxSteps = 1;
    }
    public void ButtonCrookedIsPressed()
    {
        _maxSteps = 2;
    }

    public void ButtonMeshIsPressed()
    {
        _renderMesh = true;
    }
    public void ButtonLineIsPressed()
    {
        _renderMesh = false;
    }

    public void CountLeftLanes(float lanes)
    {
        _rightLanes = lanes;
    }    
    public void CountRightLanes(float lanes)
    {
        _leftLanes = lanes;
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
                    template.enabled = true;
                    template.isStraight = true;
                    RoadEditor.MovePoint(_endPost, layerMaskGround, true);
                    break;
                case 2:
                    template.isStraight = false;
                    template.details = details;
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
                    for (int j = 0; j < _road.transform.GetChild(i).childCount; j++)
                        _road.transform.GetChild(j).GetComponent<MeshRenderer>().enabled = false;
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
        _road.transform.parent = transform;
        _road.name = "Road";

        template = _road.transform.GetComponent<TemplateRoad>();
        template.numOfLeftSideRoads = _leftLanes;
        template.numOfRightSideRoads = _rightLanes;
        template.isStraight = true;
        template.enabled = false;

        _startPost = template.startPost.transform;
        _startPost.GetComponent<MeshRenderer>().enabled = true;
        _formingPoint = template.formingPoint.transform;
        _endPost = template.endPost.transform;

        for (int i = 0; i < _road.transform.childCount; i++)
            _road.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;

        if (_renderMesh)
            _road.GetComponent<MeshVisualization>().enabled = true;
        else
            _road.GetComponent<LineVisualization>().enabled = true;
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
