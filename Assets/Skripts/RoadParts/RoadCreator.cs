using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using static GlobalSettings;
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

    public GameObject _vertexCubeRed;

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
            _startPost = Instantiate(_vertexCubeRed);
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Left click");

        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            Debug.Log("Middle click");
            eventData.Reset();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right click");
            eventData.Reset();
        }
    }

    void Update()
    {
        if (!_isEnable) return;

        RaycastHit hit;
        switch (_step)
        {
            case 1:
                if (Physics.Raycast(RayFromCursor.ray, out hit, layerMask))
                {
                    _startPost.transform.position = hit.point;
                }
                break;
        }

        if (Input.GetMouseButtonDown(0))
            Debug.Log("Pressed primary button.");

        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed secondary button.");

        if (Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click.");
    }
}
