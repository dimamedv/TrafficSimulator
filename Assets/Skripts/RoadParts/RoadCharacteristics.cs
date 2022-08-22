using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCharacteristics : MonoBehaviour
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

    // ����� ��������� ������
    public List<float> lengthSegments;

    // ���������� ����� ���� ��������� ������
    public List<float> prefixSumSegments;

    private void Awake()
    {
        points = new List<Vector3>(details);
    }
}
