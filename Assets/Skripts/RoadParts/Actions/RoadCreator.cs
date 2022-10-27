using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoadCreator : MonoBehaviour
{
    public LayerMask layerMaskGround;
    public GameObject _simpleRoadPrefab;
    public GameObject _templateRoadPrefab;
    public int details;
    public Material material;

    private AbstractRoad _abstractRoad;
    private GameObject _road;
    private Transform _startPost;
    private Transform _endPost;
    private Transform _formingPoint;
    public int _step = 0;
    private bool _isEnable = false;
    private int _maxSteps = 1;
    private bool _isStraight = true;
    private bool _renderMesh = true;
    private float _rightLanes = 1;
    private float _leftLanes = 1;

    public void ButtonStraightIsPressed()
    {
        _maxSteps = 1;
        _isStraight = true;
    }
    public void ButtonCrookedIsPressed()
    {
        _maxSteps = 2;
        _isStraight = false;
    }

    public void ButtonMeshIsPressed(bool renderMesh)
    {
        _renderMesh = renderMesh;
    }

    public void CountLeftLanes(float lanes)
    {
        _rightLanes = lanes;
    }    
    public void CountRightLanes(float lanes)
    {
        _leftLanes = lanes;
    }


    public void ButtonCreateTemplateRoad()
    {
        _isEnable = !_isEnable;

        if (_isEnable)  
            CreateTemplateRoad();
        else            
            DeleteObjects();
    }
    public void ButtonCreateSimpleRoad()
    {
        _isEnable = !_isEnable;

        if (_isEnable)
            CreateSimpleRoad();
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
                    _abstractRoad.enabled = true;
                    _abstractRoad.isStraight = true;
                    RoadEditor.MovePoint(_endPost, layerMaskGround, true);
                    break;
                case 2:
                    _abstractRoad.isStraight = false;
                    _abstractRoad.details = details;
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
                CreateTemplateRoad();
            }
        else if (Input.GetMouseButtonDown(1))
            _road.transform.GetChild(_step--).GetComponent<MeshRenderer>().enabled = false;
        else if (Input.GetMouseButtonDown(2))
            DeleteObjects();
    }

    private void CreateTemplateRoad()
    {
        gameObject.GetComponent<RoadEditor>().enabled = false;
        _road = Instantiate(_templateRoadPrefab);
        _road.transform.parent = transform;
        _road.name = "Road";

        _abstractRoad = _road.transform.GetComponent<AbstractRoad>();
        _abstractRoad.isStraight = _isStraight;
        _abstractRoad.enabled = false;

        TemplateRoad template = _road.transform.GetComponent<TemplateRoad>();
        template.numOfLeftSideRoads = _leftLanes;
        template.numOfRightSideRoads = _rightLanes;

        for (int i = 0; i < _road.transform.childCount; i++)
            _road.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;

        _startPost = _road.transform.GetChild(0);
        _startPost.GetComponent<MeshRenderer>().enabled = true;
        _endPost = _road.transform.GetChild(1);
        _formingPoint = _road.transform.GetChild(2);

        //if (_renderMesh)
            //_road.GetComponent<MeshVisualization>().enabled = true;
        //else
            //_road.GetComponent<LineVisualization>().enabled = true;
        _step = 0;
    }

    private void CreateSimpleRoad()
    {
        gameObject.GetComponent<RoadEditor>().enabled = false;
        _road = Instantiate(_simpleRoadPrefab);
        _road.name = "Road";

        _abstractRoad = _road.transform.GetComponent<AbstractRoad>();
        _abstractRoad.enabled = false;

        for (int i = 0; i < _road.transform.childCount; i++)
            _road.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;

        _startPost = _road.transform.GetChild(0);
        _startPost.GetComponent<MeshRenderer>().enabled = true;
        _endPost = _road.transform.GetChild(1);
        _formingPoint = _road.transform.GetChild(2);

        if (_renderMesh)
            _road.AddComponent<MeshVisualization>();
        else
            _road.AddComponent<LineVisualization>();
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
