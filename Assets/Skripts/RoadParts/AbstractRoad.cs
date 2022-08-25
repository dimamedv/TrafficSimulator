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

    // ���������� ����� ���� ��������� ������
    public List<float> prefixSumSegments;

    // ��� ����� ��������
    public float gridStep;

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
        if (isNeedsRebuild())
        {
            RebuildGrid();
            Debug.Log(_startPostTransform.position);
            BuildRoad();
            Debug.Log(_startPostTransform.position);
        }
    }


    protected void RebuildGridByPoint(ref GameObject t)
    {
        Vector3 a = new Vector3(
            RebuildGridByAxis(t.transform.position.x),
            0.0f,
            RebuildGridByAxis(t.transform.position.z));

        Debug.Log(a);

        t.transform.position = a;
    }

    private float RebuildGridByAxis(float x)
    {
        float remains = x % gridStep;
        if (remains < gridStep / 2) 
            return x - remains;
        else 
            return x - remains + gridStep;
    }

    protected abstract void BuildRoad();
    protected abstract bool isNeedsRebuild();
    protected abstract void RebuildGrid();
}
