using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skripts;
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
        Debug.Log(parentRoad.prefixSumSegments.Count);
        float r = distance - parentRoad.prefixSumSegments[a];
        Vector3 offset = parentRoad.angles[a] * r;
        Vector3 v = parentRoad.points[a] + offset; 
        transform.position = v;

        Vector3 rotation = new Vector3(0.0f, (float)(Math.Acos(parentRoad.angles[a].z) / Math.PI * 180) + 180, 0.0f);
        Quaternion rotationQuaternion = Quaternion.Euler(rotation);
        Debug.Log(rotationQuaternion);
        transform.rotation = rotationQuaternion;
    }
}
