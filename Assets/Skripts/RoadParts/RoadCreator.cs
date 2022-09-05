using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using static GlobalSettings;

public class RoadCreator : MonoBehaviour
{
    bool _isEnable;
    int _step;
    GameObject _road;
    GameObject _startPost;
    GameObject _endPost;

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
            _endPost = new GameObject("EndPost");
            _endPost.transform.SetParent(_road.transform, false);
            _startPost.name = "EndPost";
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

    void FixedUpdate()
    {
        if (!_isEnable) return;


    }
}
