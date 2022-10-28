using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.ConstrainedExecution;

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
    // Расстояние, пройденное машиной
    public float distance;


    // Расстояние, пройденное машиной
    public float distanceOnThisRoad;
    // Максимальная скорость автомобиля
    public float maxSpeedPerTick;
    // Ускорение в тик
    public float accelerationPerTick;
    // Торможение в секунду
    public float brakingPerTick;
    // Время, которое нужно машине для остановки
    public float brakingTime;
    // Тормозной путь
    public float brakingDistance;
    // Скорость в тик
    public float speedPerTick;


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
        if (IsItTimeToSlowDown())
            SlowDown();
        else
            SpeedUp();
        UpdateBrakingStats();
        ChangeDistance();

        if (distanceOnThisRoad >= parentRoad.prefixSumSegments[^1])
            SwitchToChild();
        TurnCar(speedPerTick);
    }

    // Пора ли тормозить?
    private bool IsItTimeToSlowDown()
    {
        // Ближайшая машина
        GameObject nearestCar = CheckoutNearestCar();
        if (nearestCar != null)
        {
            // Расстояние до ближайшей машины
            float distanceToNearestCar = nearestCar.GetComponent<CarBehaviour>().distance - this.distance;
            // Габариты этой машины
            float thisCarDimensions = gameObject.transform.localScale.x * gameObject.GetComponent<BoxCollider>().size.x / 2;
            // Габариты ближайшей машины
            float nearestCarDimensions = nearestCar.transform.localScale.x * nearestCar.GetComponent<BoxCollider>().size.x / 2;
            // Безопасная дистанция остановки
            float saveStoppingDistance = brakingDistance + GlobalSettings.SaveDistance + nearestCarDimensions + thisCarDimensions;
            // Пора ли тормозить?
            bool isItTimeToSlowDown = saveStoppingDistance > distanceToNearestCar;

            if (isItTimeToSlowDown)
            {
                return true;
            }
        }
        return false;
    }

    private GameObject CheckoutNearestCar()
    {
        GameObject nearestCar = null;
        float minDistance = float.MaxValue;
        GameObject road = parentRoad.gameObject;
        while (road != null && road.GetComponent<SimpleRoad>() != null && road.GetComponent<CrossRoadEntrance>() == null)
        {
            foreach (var car in road.GetComponent<SimpleRoad>().carsOnThisRoad)
            {
                float distanceToCar = car.GetComponent<CarBehaviour>().distance - this.distance;
                if (car != gameObject && distanceToCar > 0.0f && distanceToCar < minDistance)
                {
                    minDistance = distanceToCar;
                    nearestCar = car;
                }
            }
            road = road.GetComponent<SimpleRoad>().parentConnection;
        }
        return nearestCar;
    }

    private void SlowDown()
    {
        if (speedPerTick - brakingPerTick > 0.0f)
            speedPerTick -= brakingPerTick;
        else
            speedPerTick = 0.0f;
    }

    private void SpeedUp()
    {
        if (speedPerTick + accelerationPerTick < maxSpeedPerTick)
            speedPerTick += accelerationPerTick;
        else
            speedPerTick = maxSpeedPerTick;
    }

    private void UpdateBrakingStats()
    {
        brakingTime = speedPerTick / brakingPerTick;
        brakingDistance = speedPerTick * brakingTime / 2;
    }

    private void ChangeDistance()
    {
        distanceOnThisRoad += speedPerTick;
        distance += speedPerTick;
    }

    private void SwitchToChild()
    {
        distanceOnThisRoad -= parentRoad.prefixSumSegments[^1];
        if (!parentRoad.childConnection)
        {
            parentRoad.carsOnThisRoad.Remove(gameObject);
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
