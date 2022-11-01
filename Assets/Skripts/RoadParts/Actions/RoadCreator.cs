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
    public LayerMask layerMaskUI; // Слой UI

    private AbstractRoad _abstractRoad;
    private GameObject _road;
    private Transform _startPost;
    private Transform _endPost;
    private Transform _formingPoint;
    public int _step = 0;
    private bool _isEnable = false;
    private int _maxSteps = 1;
    private bool _isStraight = true;
    private bool _renderLine = false;
    public float _rightLanes = 1;
    public float _leftLanes = 1;
    private bool _isTemplate;

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

    public void ButtonMeshIsPressed(bool renderLine)
    {
        _renderLine = renderLine;
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
        _isTemplate = true;

        if (_isEnable)  
            CreateTemplateRoad();
        else            
            DeleteObjects();
    }
    public void ButtonCreateSimpleRoad()
    {
        _isEnable = !_isEnable;
        _isTemplate = false;

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
        if (Input.GetMouseButtonDown(0) && !Physics.Raycast(RayFromCursor.ray, 1000, layerMaskUI))
            if (_step < _maxSteps)
                _road.transform.GetChild(++_step).GetComponent<MeshRenderer>().enabled = true;
            else
            {
                for (int i = 0; i < _road.transform.childCount; i++)
                    for (int j = 0; j < _road.transform.GetChild(i).childCount; j++)
                        _road.transform.GetChild(j).GetComponent<MeshRenderer>().enabled = false;
                if (_isTemplate)
                    CreateTemplateRoad();
                else
                    CreateSimpleRoad();
            }
        else if (Input.GetMouseButtonDown(1) && !Physics.Raycast(RayFromCursor.ray, 1000, layerMaskUI))
            _road.transform.GetChild(_step--).GetComponent<MeshRenderer>().enabled = false;
        else if (Input.GetMouseButtonDown(2))
            DeleteObjects();
    }

    private void CreateTemplateRoad()
    {
        gameObject.GetComponent<RoadEditor>().enabled = false;
        _road = Instantiate(_templateRoadPrefab);
        _road.transform.parent = transform;
        _road.name = "TemplateRoad";

        TemplateRoad template = _road.transform.GetComponent<TemplateRoad>();
        template.numOfLeftSideRoads = _leftLanes;
        template.numOfRightSideRoads = _rightLanes;
        template.isStraight = _isStraight;
        template.render(false);
        template.enabled = false;

        for (int i = 0; i < 3; i++)
            _road.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;

        _startPost = _road.transform.GetChild(0);
        _startPost.GetComponent<MeshRenderer>().enabled = true;
        _endPost = _road.transform.GetChild(1);
        _formingPoint = _road.transform.GetChild(2);

        _step = 0;
    }

    private void CreateSimpleRoad()
    {
        gameObject.GetComponent<RoadEditor>().enabled = false;
        _road = Instantiate(_simpleRoadPrefab);
        _road.name = "Road";
        _road.GetComponent<SimpleRoad>().id =
            GameObject.Find("Game").GetComponent<GlobalSettings>().GetNextRoadNumeration();

        _abstractRoad = _road.transform.GetComponent<AbstractRoad>();
        _abstractRoad.enabled = false;

        for (int i = 0; i < 3; i++)
            _road.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;

        _startPost = _road.transform.GetChild(0);
        _startPost.GetComponent<MeshRenderer>().enabled = true;
        _endPost = _road.transform.GetChild(1);
        _formingPoint = _road.transform.GetChild(2);

        if (_renderLine)
            _road.GetComponent<SimpleRoad>().renderLine = _renderLine;

        _step = 0;
    }

    private void DeleteObjects()
    {
        AbstractRoad.RoadList.Remove(_road);
        Destroy(_road);
        gameObject.GetComponent<RoadEditor>().enabled = true;
        _step = 0;
        _isEnable = false;
    }
}
