using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarBehaviour : MonoBehaviour
{
    // Максимальная скорость автомобиля
    public float maxSpeedPerSec;
    // Ускорение в секунду
    public float accelerationPerSec;
    // Торможение в секунду
    public float brakingPerSec;
    // Дорога, по которой едет авто
    public SimpleRoad parentRoad;

    // Максимальная скорость автомобиля
    private float maxSpeedPerTick;
    // Ускорение в тик
    private float accelerationPerTick;
    // Торможение в секунду
    public float brakingPerTick;
    // Время, которое нужно машине для остановки
    private float brakingTime;
    // Тормозной путь
    private float brakingDistances;
    // Расстояние, пройденное машиной
    private float distance;
    // Скорость в тик
    private float speedPerTick;


    // Событие, которое вызывается в случае столкновения машин
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Car")
            Debug.Log("Машины столкнулись!!!");
    }

    private void Awake()
    {
        maxSpeedPerTick = maxSpeedPerSec * Time.deltaTime;
        accelerationPerTick = accelerationPerSec * Time.deltaTime;
        brakingPerTick = brakingPerSec * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        UpdateBrakingStats();
        ChangeSpeed();
        if (distance >= parentRoad.prefixSumSegments[^1])
            SwitchToChild();
        TurnCar(speedPerTick * Time.deltaTime);
    }

    private void UpdateBrakingStats()
    {
        brakingTime = speedPerTick / brakingPerTick;
        brakingDistances = speedPerTick * brakingTime / 2;
    }

    private void ChangeSpeed()
    {
        if (speedPerTick + accelerationPerTick < maxSpeedPerSec)
            speedPerTick += accelerationPerTick;
        else
            speedPerTick = maxSpeedPerSec;

        distance += speedPerTick * Time.deltaTime;
    }

    private void SwitchToChild()
    {
        distance -= parentRoad.prefixSumSegments[^1];
        if (!parentRoad.childConnection)
        {
            Destroy(gameObject);
        }
        else if (parentRoad.childConnection.GetComponent<SimpleRoad>())
        {
            parentRoad = parentRoad.childConnection.GetComponent<SimpleRoad>();
            parentRoad.carsOnThisRoad.Add(gameObject);
        }
        else if (parentRoad.childConnection.GetComponent<CrossRoadEntrance>() &&
                  parentRoad.childConnection.GetComponent<CrossRoadEntrance>().childRoads.Count != 0) // Соединение на перекрестке
        {                                                                                             // По дефолту едет на первый съезд
            parentRoad = parentRoad.childConnection.GetComponent<CrossRoadEntrance>().childRoads[0].GetComponent<SimpleRoad>();
        }
        else
        {
            Destroy(gameObject);
        }
    }



    private void TurnCar(float speed)
    {
        int nextPointIndex = MyMath.binarySearch(ref parentRoad.prefixSumSegments, parentRoad.prefixSumSegments.Count, distance);
        transform.LookAt(new Vector3(parentRoad.points[nextPointIndex].x, parentRoad.points[nextPointIndex].y + transform.position.y,
            parentRoad.points[nextPointIndex].z));
        transform.Rotate(-Vector3.up * 90);

        transform.position += transform.right * speed;
    }

    private float getOptimalSpeed()
    {
        return 0;
    }
    
    
}
