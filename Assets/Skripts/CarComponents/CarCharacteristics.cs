using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCharacteristics : MonoBehaviour
{
    // ������������ ��������
    public float maxSpeed;
    // ���������
    public float acceleration;
    // �������� ���������� (���� �� ������ ��������, ������� � ������)
    private Vector3 size;
    // �������� � �������
    public float speed;
    // ������, ������� ����������� ����
    public RoadCharacteristics parentRoad;
    // ����������, ������� ������ ������ �� ������
    public float distance;
}
