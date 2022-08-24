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
    protected Vector3 curStartPosition;
    protected Vector3 curEndPosition;
    
    

    public List<GameObject> carsOnThisRoad;
    
    public void Awake()
    {
        _startPostTransform = transform.Find("StartPost").transform;
        _endPostTransform = transform.Find("EndPost").transform;
        curStartPosition = _startPostTransform.position;
        curEndPosition = _endPostTransform.position;

        BuildRoad();
    }

    void FixedUpdate()
    {
        if (points[0] != _startPostTransform.position || points[^1] != _endPostTransform.position)
        {
            BuildRoad();
        }
    }


    protected abstract void BuildRoad();
    protected abstract bool NeedsRebuild();
}
