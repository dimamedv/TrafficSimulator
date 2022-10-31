using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FrameRoadsSelector;

public class CarBehaviourOnCrossroad : CarBehaviour
{
    public int currentFrame = 0;
    GameObject roadFather;

    public void Start()
    {
        roadFather = GameObject.Find("RoadFather");
    }

    // Пора ли тормозить?
    public override bool IsItTimeToSlowDown()
    {
        // Ближайшая машина
        float nearestCarDistance = GetNearestCarDistance();
        int nearestCrossroadId = 0;
        float nearestCrossroadDistance = -distance;

        if (path.Count == 0)
            nearestCrossroadDistance = float.MaxValue;
        for (int i = path.Count - 1; i >= 0; i--)
        {
            nearestCrossroadDistance += path[i].GetComponent<SimpleRoad>().prefixSumSegments[^1];
            if (roadFather.GetComponent<FrameRoadsSelector>().CheckIfIsEntranceToCrossRoad(path[i]))
            {
                nearestCrossroadId = i;
                break;
            }
        }

        if (nearestCarDistance < nearestCrossroadDistance)
            return nearestCarDistance < brakingDistance + GlobalSettings.SaveDistance;

        if (nearestCrossroadDistance > brakingDistance)
            return false;

        float timeToNearestCrossroad = TimeToPass(nearestCrossroadDistance);
        if (timeToNearestCrossroad > 30.0f)
            return true;

        if (roadFather.GetComponent<RelationsEditor>().frames[currentFrame].GetRoadToTrackById(nearestCrossroadId)
                .Count == 0)
            return false;

        int idRoad = path[nearestCrossroadId].GetComponent<SimpleRoad>().id;
        FrameRoadsSelector frameRoadsSelector = roadFather.GetComponent<FrameRoadsSelector>();
        foreach (var road in frameRoadsSelector.frames[frameRoadsSelector.currentFrame].GetRoadToTrackById(idRoad))
        {
            SimpleRoad simpleRoad = frameRoadsSelector.frames[frameRoadsSelector.currentFrame].GetRoadById(road)
                .GetComponent<SimpleRoad>();
            SimpleRoad simpleParent = simpleRoad.childConnection.GetComponent<SimpleRoad>();
            float distanceCar = simpleRoad.prefixSumSegments[^1];
            while (simpleParent != null)
            {
                distanceCar += simpleParent.prefixSumSegments[^1];
                if (simpleParent.carsOnThisRoad.Count > 0)
                {
                    float timeForCar = TimeToPass(distanceCar - simpleParent.carsOnThisRoad[0]
                        .GetComponent<CarBehaviourOnCrossroad>().TimeToPass(distanceCar));
                    if (timeForCar + 2.0f < timeToNearestCrossroad)
                        return true;
                }
            }
        }

        return false;
    }


    // 
    private float GetNearestCarDistance()
    {
        GameObject nearestCar = FindNearestCar();
        if (nearestCar != null)
        {
            // Габариты этой машины
            float thisCarDimensions =
                gameObject.transform.localScale.x * gameObject.GetComponent<BoxCollider>().size.x / 2;
            // Габариты ближайшей машины
            float nearestCarDimensions =
                nearestCar.transform.localScale.x * nearestCar.GetComponent<BoxCollider>().size.x / 2;
            // Расстояние до ближайшей машины
            float distanceToNearestCar = nearestCar.GetComponent<CarBehaviour>().distance - this.distance -
                                         thisCarDimensions - nearestCarDimensions;

            return distanceToNearestCar;
        }

        return float.MaxValue;
    }

    // Возвращает ближайшую машину на пути
    private GameObject FindNearestCar()
    {
        for (int i = path.Count - 1; i >= 0; i--)
        {
            foreach (var car in path[i].GetComponent<SimpleRoad>().carsOnThisRoad)
            {
                float distanceToCar = car.GetComponent<CarBehaviour>().distance - this.distance;
                if (car != gameObject && distanceToCar > 0.0f)
                    return car;
            }
        }

        return null;
    }

    public float TimeToPass(float distance)
    {
        float equidistantTime = (maxSpeedPerTick - speedPerTick) / accelerationPerTick;
        float equidistantDistance =
            speedPerTick * equidistantTime + accelerationPerTick * equidistantTime * equidistantTime / 2;
        float equidimensionalTime = (distance - equidistantDistance) / maxSpeedPerTick;
        return equidistantTime + equidimensionalTime;
    }
}