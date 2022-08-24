using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractRoad : MonoBehaviour
{
    // ��������� �����
    public GameObject _startPost;

    // �������� �����
    public GameObject _endPost;

    // ������ ������
    public float width;

    // ���������� ���������� ������ (�����������)
    public int details;

    // �����, ����� ������� �������� ����������
    public List<Vector3> points;

    // ���� �� ��������� �����. (cosA, 0, sinA)
    public List<Vector3> angles;

    // ����� ��������� ������
    public List<float> lengthSegments;

    // ���������� ����� ���� ��������� ������
    public List<float> prefixSumSegments;

    protected Transform _startPostTransform;
    protected Transform _endPostTransform;
    protected Vector3 startPostPosition;
    protected Vector3 endPostPosition;
    
    

    public List<GameObject> carsOnThisRoad;
    
    void Awake()
    {
        _startPostTransform = transform.Find("StartPost").transform;
        _endPostTransform = transform.Find("EndPost").transform;
        startPostPosition = _startPostTransform.position;
        endPostPosition = _endPostTransform.position;

        BuildRoad();
    }
    
    void FixedUpdate()
    {
        startPostPosition = _startPostTransform.position;
        endPostPosition = _endPostTransform.position;

        if (points[0] != startPostPosition || points[^1] != endPostPosition)
        {
            BuildRoad();
        }
    }


    protected abstract void BuildRoad();
}
