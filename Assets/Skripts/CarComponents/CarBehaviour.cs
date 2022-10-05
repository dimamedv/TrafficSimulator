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
    public SimpleRoad parentRoad;
    // ����������, ������� ������ ������ �� ������
    public float distance;
    // ��������� � ���
    private float accelerationPerTick;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Car")
            Debug.Log("Машины столкнулись!!!");
    }

    private void Awake()
    {
        accelerationPerTick = acceleration * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (speed + accelerationPerTick < maxSpeed) 
            speed += accelerationPerTick;
        else 
            speed = maxSpeed;
        
        distance += speed * Time.deltaTime;

        int nextPointIndex = MyMath.binarySearch(ref parentRoad.prefixSumSegments, parentRoad.prefixSumSegments.Count, distance);
        transform.LookAt(new Vector3(parentRoad.points[nextPointIndex].x, parentRoad.points[nextPointIndex].y + transform.position.y,
            parentRoad.points[nextPointIndex].z));
        transform.Rotate(-Vector3.up * 90);

        transform.position += transform.right * (speed * Time.deltaTime);
        
        if (distance >= parentRoad.prefixSumSegments[^1])
        {
            distance = 0;
            if (parentRoad.childConnection != null)
                parentRoad = parentRoad.childConnection.GetComponent<SimpleRoad>();
            else 
                Destroy(gameObject);
        }
    }

    private float getOptimalSpeed()
    {
        return 0;
    }
    
    
}
