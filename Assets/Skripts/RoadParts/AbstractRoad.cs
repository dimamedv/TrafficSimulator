using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractRoad : MonoBehaviour
{
    public GameObject startPost; // ��������� �����
    public GameObject endPost; // �������� �����
    public GameObject parentPost; // ��������
    public GameObject childPost; // �������
    public float width; // ������ ������
    public List<Vector3> points; // �����, ����� ������� �������� ����������
    public List<float> prefixSumSegments; // ���������� ����� ���� ��������� ������
    public float gridStep; // ��� ����� ��������
    public List<Vector3> angles; // ���� �� ��������� �����. (cosA, 0, sinA)
    public List<GameObject> carsOnThisRoad;


    public void Awake()
    {
        startPost = transform.GetChild(0).gameObject;
        endPost = transform.GetChild(1).gameObject;

        BuildRoad();
    }

    void FixedUpdate()
    {
        if (NeedsRebuild())
        {
            BuildRoad();
        }
    }


    protected void RebuildGridByPoint(ref GameObject t)
    {
        var position = t.transform.position;
        position = new Vector3(
            RebuildGridByAxis(position.x),
            0.0f,
            RebuildGridByAxis(position.z));
        t.transform.position = position;
    }

    private float RebuildGridByAxis(float x)
    {
        float remains = x % gridStep;
        if (remains < gridStep / 2)
            return x - remains;
        else
            return x - remains + gridStep;
    }
    
    protected  void RebuildGrid()
    {
        RebuildGridByPoint(ref startPost);
        RebuildGridByPoint(ref endPost);
    }

    
    protected abstract void BuildRoad();
    protected abstract bool NeedsRebuild();
}