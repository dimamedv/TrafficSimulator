using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarBehaviour : MonoBehaviour
{
    // ������������ ��������
    public float maxSpeed;
    // ��������� � �������
    public float acceleration;
    // �������� � �������
    public float speed;
    // ������, ������� ����������� ����
    public AbstractRoad parentRoad;
    // ����������, ������� ������ ������ �� ������
    public float distance;
    // ��������� � ���
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
        float r = distance - parentRoad.prefixSumSegments[a];
        Vector3 offset = parentRoad.angles[a] * r;
        Vector3 v = parentRoad.points[a] + offset; 
        transform.position = v;

        Vector3 rotation = new Vector3(0.0f, (float)(Math.Acos(parentRoad.angles[a].z) / Math.PI * 180) - 90, 0.0f);
        Quaternion rotationQuaternion = Quaternion.Euler(rotation);
        
        transform.rotation = rotationQuaternion;
    }
    
    
}
