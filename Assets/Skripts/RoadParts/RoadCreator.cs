using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoadCreator : MonoBehaviour
{
    bool _isEnable;
    int _step;
    public LayerMask layerMask;

    GameObject _road;
    GameObject _startPost;
    GameObject _endPost;
    GameObject _formingPoint;

    public GameObject cubeRed;
    public GameObject cubeGreen;
    public GameObject cubeBlue;


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
            _road = new GameObject("CrookedRoad");
            _startPost = Instantiate(cubeRed);
            _startPost.transform.SetParent(_road.transform, false);
            _startPost.name = "StartPost";
            _step++;
        }
        else
        {
            Destroy(_road);
            Destroy(_startPost);
            Destroy(_endPost);
            _step = 0;
        }

    }

    void Update()
    {
        if (!_isEnable) return;

        RaycastHit hit;
        switch (_step)
        {
            case 1:
                if (Physics.Raycast(RayFromCursor.ray, out hit, 1000, layerMask))
                    _startPost.transform.position = hit.point;
                break;
            case 2:
                _endPost = Instantiate(cubeGreen);
                _endPost.transform.SetParent(_road.transform, false);
                _endPost.name = "EndPost";

                _formingPoint = Instantiate(cubeBlue);
                _formingPoint.transform.SetParent(_road.transform, false);
                _formingPoint.name = "FormingPoint";

                if (Physics.Raycast(RayFromCursor.ray, out hit, 1000, layerMask))
                    _endPost.transform.position = hit.point;

                break;
        }

        if (Input.GetMouseButtonDown(0))
            _step++;

        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed secondary button.");
        
        if (Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click.");
    }
}
