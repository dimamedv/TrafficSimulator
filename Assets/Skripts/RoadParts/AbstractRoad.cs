using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractRoad : MonoBehaviour
{
    // ��������� �����
    public GameObject _startPost;

    // �������� �����
    public GameObject _endPost;

    // ��������
    public GameObject _parentPost;

    // �������
    public GameObject _childPost;

    // ������ ������
    public float width;

    // ���������� ���������� ������ (�����������)
    public int details;

    // �����, ����� ������� �������� ����������
    public List<Vector3> points;

    // ���������� ����� ���� ��������� ������
    public List<float> prefixSumSegments;

    // ��� ����� ��������
    public float gridStep;
    
    // ���� �� ��������� �����. (cosA, 0, sinA)
    public List<Vector3> angles;

    protected Vector3 curStartPosition;
    protected Vector3 curEndPosition;
    
    

    public List<GameObject> carsOnThisRoad;
    
    public void Awake()
    {
        _startPost = transform.GetChild(0).gameObject;
        _endPost = transform.GetChild(1).gameObject;
        curStartPosition = _startPost.transform.position;
        curEndPosition = _endPost.transform.position;

        BuildRoad();
    }

    void FixedUpdate()
    {
        if (isNeedsRebuild())
        {
            BuildRoad();
        }
    }


    protected void RebuildGridByPoint(ref GameObject t) { 
        t.transform.position = new Vector3(
            RebuildGridByAxis(t.transform.position.x),
            0.0f,
            RebuildGridByAxis(t.transform.position.z));
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
