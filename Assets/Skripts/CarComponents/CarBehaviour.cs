using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarBehaviour : MonoBehaviour
{
    // Максимальная скорость
    public float maxSpeed;
    // Ускорение в секунду
    public float acceleration;
    // Скорость в секунду
    public float speed;
    // Дорога, которой принадлежит авто
    public AbstractRoad parentRoad;
    // Расстояние, которое прошла машина по дороге
    public float distance;
    // Ускорение в тик
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

        transform.LookAt(parentRoad.points[a]);
        transform.Rotate(-Vector3.up * 90);

        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
    
    
}
