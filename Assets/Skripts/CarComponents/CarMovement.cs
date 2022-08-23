using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarMovement : MonoBehaviour
{
    private CarCharacteristics car;

    private float accelerationPerTick;

    private float speedPerTick;

    private void Awake()
    {
        car = GetComponent<CarCharacteristics>();
        accelerationPerTick = car.acceleration * Time.deltaTime;
        speedPerTick = car.speed * Time.deltaTime;
        Debug.Log(Time.deltaTime);
        Debug.Log(car.acceleration);
        Debug.Log(accelerationPerTick);
    }

    private static float eps = 0.001f;

    public static int binarySearch(List<float> a, float x)
    {
        int left = 0;
        int right = a.Count - 1;
        while (Math.Abs(left - right) <= eps)
        {
            int mid = (left + right) / 2;
            if (Math.Abs(a[mid] - x) < eps) left = mid + 1;
            else if (Math.Abs(a[mid] - x) > eps) right = mid - 1;
            else return mid;
        }
        return left;
    }

    private void FixedUpdate()
    {
        if (car.speed + accelerationPerTick < car.maxSpeed) car.speed += accelerationPerTick;
        else car.speed = car.maxSpeed;
        GetComponent<CarCharacteristics>().speed = car.speed;

        car.distance += car.speed;
        GetComponent<CarCharacteristics>().distance = car.distance;
    }
}
