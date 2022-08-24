using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skripts;
using System;

public class CarBehaviour : MonoBehaviour
{
    // Максимальная скорость
    public float maxSpeed;
    // Ускорение
    public float acceleration;
    // Скорость в моменте
    public float speed;
    // Дорога, которой принадлежит авто
    public AbstractRoad parentRoad;
    // Расстояние, которое прошла машина по дороге
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
