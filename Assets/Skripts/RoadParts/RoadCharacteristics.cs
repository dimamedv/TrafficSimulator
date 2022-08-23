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

    // ���� �� ��������� �����. (cosA, 0, sinA)
    public List<Vector3> angles;

    // ����� ��������� ������
    public List<float> lengthSegments;

    // ���������� ����� ���� ��������� ������
    public List<float> prefixSumSegments;
}
