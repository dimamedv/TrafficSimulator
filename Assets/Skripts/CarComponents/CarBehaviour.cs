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
    Transform crossroadEntrance;

    public GameObject destinationPost;
    public List<GameObject> path;


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

        path = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        if (IsItTimeToSlowDown())
            SlowDown();
        else
            SpeedUp();
        UpdateBrakingStats();
        ChangeDistance();

        crossroadEntrance = parentRoad.transform.Find("CrossRoadEntrance");
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
            float thisCarDimensions =
                gameObject.transform.localScale.x * gameObject.GetComponent<BoxCollider>().size.x / 2;
            // Габариты ближайшей машины
            float nearestCarDimensions =
                nearestCar.transform.localScale.x * nearestCar.GetComponent<BoxCollider>().size.x / 2;
            // Безопасная дистанция остановки
            float saveStoppingDistance =
                brakingDistance + GlobalSettings.SaveDistance + nearestCarDimensions + thisCarDimensions;
            // Пора ли тормозить?
            bool isItTimeToSlowDown = saveStoppingDistance > distanceToNearestCar;

            if (isItTimeToSlowDown)
            {
                return true;
            }
        }

        return false;
    }

    // Возвращает блтжайшую машину на пути
    private GameObject CheckoutNearestCar()
    {
        GameObject nearestCar = null;
        float minDistance = float.MaxValue;
        Transform crossroadEntrancePtr = crossroadEntrance;
        while (crossroadEntrancePtr != null &&
               crossroadEntrancePtr.GetComponent<CrossRoadEntrance>().childRoads.Count > 0)
        {
            foreach (var car in crossroadEntrance.GetComponent<CrossRoadEntrance>().childRoads[0]
                         .GetComponent<SimpleRoad>().carsOnThisRoad)
            {
                Debug.Log(car.GetComponent<CarBehaviour>().distance);
                float distanceToCar = car.GetComponent<CarBehaviour>().distance - this.distance;
                if (car != gameObject && distanceToCar > 0.0f && distanceToCar < minDistance)
                {
                    minDistance = distanceToCar;
                    nearestCar = car;
                }
            }

            crossroadEntrancePtr = crossroadEntrance.GetComponent<CrossRoadEntrance>().childRoads[0]
                .GetComponent<SimpleRoad>().transform.Find("CrossRoadEntrance");
        }

        return nearestCar;
    }

    // Уменьшает скорость машины в этом тике
    private void SlowDown()
    {
        if (speedPerTick - brakingPerTick > 0.0f)
            speedPerTick -= brakingPerTick;
        else
            speedPerTick = 0.0f;
    }

    // Увеличивает скорость машины в этом тике
    private void SpeedUp()
    {
        if (speedPerTick + accelerationPerTick < maxSpeedPerTick)
            speedPerTick += accelerationPerTick;
        else
            speedPerTick = maxSpeedPerTick;
    }

    // Обновляет тормозные характеристики машины
    private void UpdateBrakingStats()
    {
        brakingTime = speedPerTick / brakingPerTick;
        brakingDistance = speedPerTick * brakingTime / 2;
    }

    // Изменяет дистанцию, пройденную автомобилем
    private void ChangeDistance()
    {
        distanceOnThisRoad += speedPerTick;
        distance += speedPerTick;
    }

    // 
    private void SwitchToChild()
    {
        distanceOnThisRoad -= parentRoad.prefixSumSegments[^1];
        if (crossroadEntrance != null && crossroadEntrance.GetComponent<CrossRoadEntrance>().childRoads.Count != 0)
        {
            parentRoad = crossroadEntrance.GetComponent<CrossRoadEntrance>().childRoads[0].GetComponent<SimpleRoad>();
        }
        else
        {
            DeleteCar();
        }
    }


    private void TurnCar(float speed)
    {
        int nextPointIndex = MyMath.binarySearch(ref parentRoad.prefixSumSegments, parentRoad.prefixSumSegments.Count,
            distance);
        transform.LookAt(new Vector3(parentRoad.points[nextPointIndex].x,
            parentRoad.points[nextPointIndex].y + transform.position.y,
            parentRoad.points[nextPointIndex].z));
        transform.Rotate(-Vector3.up * 90);

        transform.position += transform.right * speed;
    }

    private void DeleteCar()
    {
        parentRoad.carsOnThisRoad.Remove(gameObject);
        Destroy(gameObject);
    }

    private void ChangeRoad()
    {
        parentRoad.carsOnThisRoad.Remove(gameObject);
        parentRoad = crossroadEntrance.GetComponent<CrossRoadEntrance>().childRoads[0].GetComponent<SimpleRoad>();
        parentRoad.carsOnThisRoad.Add(gameObject);
    }

    private float getOptimalSpeed()
    {
        return 0;
    }

    public bool findPathToDestination(GameObject currentRoad)
    {
        if (destinationPost == null)
            return false;
        
        SimpleRoad curRoadScript = currentRoad.GetComponent<SimpleRoad>();
        
        if (curRoadScript.endPost == destinationPost)
        {
            path.Clear();
            path.Add(currentRoad);
            return true;
        }
        
        if (curRoadScript.childConnection)
        {
            if (curRoadScript.childConnection.GetComponent<SimpleRoad>())
            {
                if (findPathToDestination(curRoadScript.childConnection))
                {
                    path.Add(currentRoad);
                    return true;
                }
            }
            else if (curRoadScript.childConnection.GetComponent<CrossRoadEntrance>())
            {
                foreach (var road in curRoadScript.childConnection.GetComponent<CrossRoadEntrance>().childRoads)
                {
                    if (findPathToDestination(road))
                    {
                        path.Add(currentRoad);
                        return true;
                    }
                }
            }
        }

        return false;
    }


    public GameObject getNextRoad()
    {
        GameObject next = path[^1];
        path.RemoveAt(path.Count - 1);

        return next;
    }
}