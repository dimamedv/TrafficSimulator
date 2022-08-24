using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skripts;
using System;

public class CarBehaviour : MonoBehaviour
{
    // ������������ ��������
    public float maxSpeed;
    // ���������
    public float acceleration;
    // �������� � �������
    public float speed;
    // ������, ������� ����������� ����
    public AbstractRoad parentRoad;
    // ����������, ������� ������ ������ �� ������
    public float distance;

    private float accelerationPerTick;

    private void Awake()
    {
        accelerationPerTick = acceleration * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (speed + accelerationPerTick < maxSpeed) speed += accelerationPerTick;
        else speed = maxSpeed;
        
        distance += speed * Time.deltaTime;

        int a = MyMath.binarySearch(ref parentRoad.prefixSumSegments, parentRoad.prefixSumSegments.Count, distance);
        Debug.Log(parentRoad.prefixSumSegments.Count);
        Debug.Log(a);
        Debug.Log(distance);
        float r = distance - parentRoad.prefixSumSegments[a];
        Vector3 offset = parentRoad.angles[a] * r;
        Vector3 v = parentRoad.points[a] + offset; 
        transform.position = v;
    }
}
