using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoadCreator : MonoBehaviour
{
    public LayerMask layerMask;
    public GameObject _templateRoadPrefab;
    public GameObject _simpleRoadPrefab;
    public int details;
    public Material material;

    private TemplateRoad _template;
    private GameObject _road;
    private GameObject simpleRoadsFolder;
    private Transform _startPost;
    private Transform _endPost;
    private Transform _formingPoint;
    private int _step = 0;
    private bool _isEnable = false;
    private int _maxSteps = 1;


    // Событие, которое активирует перестройку прямой дороги
    public void ButtonStraightIsPressed()
    {
        _maxSteps = 1;
    }

    // Событие, которое активирует перестройку кривой дороги
    public void ButtonCrookedIsPressed()
    {
        _maxSteps = 2;
    }

    // Событие, которое активирует перестройку дороги
    public void ButtonCreateRoadIsPressed(int countLanes)
    {
        _isEnable = !_isEnable;

        if (_isEnable)  
            CreateRoadSkeleton(countLanes);
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
    
    // Обновляет положение активного объекта, на позицую, куда указывает курсор
    private void UpdateObjectPosToCursorPos()
    {
        switch (_step)
            {
                case 0:
                    MovePoint(_startPost, layerMask, true);
                    break;
                case 1:
                    _template.enabled = true;
                    _template.isStraight = true;
                    MovePoint(_endPost, layerMask, true);
                    break;
                case 2:
                    _template.isStraight = false;
                    _template.details = details;
                    MovePoint(_formingPoint, layerMask, false);
                    break;
            }
        
    }

    // Передвигает объект _transform на ту позицию, куда падает луч из курсора в слое _layerMask.
    // Если _rebuildPointByGrid - правда, то округляет позицию объекта до числа, кратного шагу сетки.
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

    // Проверяет какая кнопка была нажата
    private void CheckMouseButton()
    {
        if (Input.GetMouseButtonDown(0))
            if (_step < _maxSteps)
                _road.transform.GetChild(++_step).GetComponent<MeshRenderer>().enabled = true;
            else
            {
                for (int i = 0; i < _road.transform.childCount - 1; i++)
                    _road.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
                CreateRoadSkeleton(_template._countLanes);
            }
        else if (Input.GetMouseButtonDown(1))
            _road.transform.GetChild(_step--).GetComponent<MeshRenderer>().enabled = false;
        else if (Input.GetMouseButtonDown(2))
            DeleteObjects();
    }

    private void CreateRoadSkeleton(int countLanes = 1)
    {
        gameObject.GetComponent<RoadEditor>().enabled = false;
        _road = Instantiate(_templateRoadPrefab);
        _road.name = "Road";
        _startPost = _road.transform.Find("StartPost");
        _endPost = _road.transform.Find("EndPost");
        _formingPoint = _road.transform.Find("FormingPoint");
        TemplateRoad.TurnOffPoints(_road);
        //for (int i = 0; i < _road.transform.childCount; i++)
            //_road.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
        _template = _road.transform.GetComponent<TemplateRoad>();
        _template.enabled = false;
        _template._countLanes = countLanes;
        _startPost.GetComponent<MeshRenderer>().enabled = true;
        _step = 0;

        createSimpleRoads(countLanes);
    }
    private void createSimpleRoads(int countLanes)
    {
        simpleRoadsFolder = new GameObject("SimpleRoads");
        simpleRoadsFolder.transform.parent = _road.transform;

        if (countLanes == 1)
        {
            InstantiateSimpleRoad("Right0");
        }
        else
        {
            for (int i = 0; i < countLanes / 2; i++)
            {
                InstantiateSimpleRoad("Right" + i);
                InstantiateSimpleRoad("Left" + i);
            }
        }
    }

    private void InstantiateSimpleRoad(string name)
    {
        _template.DictOfSimpleRoads[name] = Instantiate(_simpleRoadPrefab, simpleRoadsFolder.transform);
        _template.DictOfSimpleRoads[name].transform.name = name;
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
