using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.ConstrainedExecution;
using UnityEngine.UIElements;
using static GlobalSettings;
public abstract class CarBehaviour : MonoBehaviour
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

    public Transform crossroadEntrance;
    public GameObject destinationPost;
    public List<GameObject> path = new List<GameObject>();

    public float nearestCrossroadDistance;
    public float crossroadEnd;
    public GameObject nearestCrossroad;

    public abstract bool IsItTimeToSlowDown();

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


        crossroadEntrance = parentRoad.transform.Find("CrossRoadEntrance");
        if (distanceOnThisRoad >= parentRoad.prefixSumSegments[^1])
            SwitchToChild();
        TurnCar(speedPerTick);
    }

    public void GetNearestCrossroad()
    {
        crossroadEnd = parentRoad.prefixSumSegments[^1];
        for (int i = path.Count - 1; i >= 0; i--)
        {
            crossroadEnd += path[i].GetComponent<SimpleRoad>().prefixSumSegments[^1];
            var road = path[i].GetComponent<SimpleRoad>().parentConnection;
            if (roadFather.GetComponent<FrameRoadsSelector>().CheckIfIsEntranceToCrossRoad(road))
            {
                nearestCrossroadDistance = crossroadEnd - path[i].GetComponent<SimpleRoad>().prefixSumSegments[^1];
                nearestCrossroad = path[i];
                return;
            }
        }
        nearestCrossroadDistance = float.MaxValue;
        crossroadEnd = float.MaxValue;
        nearestCrossroad = null;
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
        nearestCrossroadDistance -= speedPerTick;
        if (nearestCrossroadDistance < 0)
            nearestCrossroadDistance = float.MaxValue;

        crossroadEnd -= speedPerTick;
        if (crossroadEnd < 0)
            crossroadEnd = float.MaxValue;

        distanceOnThisRoad += speedPerTick;
        distance += speedPerTick;
    }

    // 
    private void SwitchToChild()
    {
        distanceOnThisRoad -= parentRoad.prefixSumSegments[^1];
        if (path.Count > 0) 
        {
            ChangeRoad();
        }
        else
        {
            DeleteCar();
        }
    }


    private void TurnCar(float speed)
    {
        int nextPointIndex = MyMath.binarySearch(ref parentRoad.prefixSumSegments, parentRoad.prefixSumSegments.Count,
            distanceOnThisRoad);
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
        parentRoad = getNextRoad().GetComponent<SimpleRoad>();
        parentRoad.carsOnThisRoad.Add(gameObject);
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