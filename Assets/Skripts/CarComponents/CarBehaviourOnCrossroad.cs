using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FrameRoadsSelector;

public class CarBehaviourOnCrossroad : CarBehaviour
{
    public int currentFrame = 0;
    GameObject roadFather;

    public float nearestCarDistance;

    // Пора ли тормозить?
    public override bool IsItTimeToSlowDown()
    {
        nearestCarDistance = GetNearestCarDistance();

        if (path.Count == 0)
            nearestCrossroadDistance = float.MaxValue;
        
        //GetNearestCrossroad();

        if (nearestCarDistance < nearestCrossroadDistance)
            return nearestCarDistance < brakingDistance + GlobalSettings.SaveDistance;

        if (nearestCrossroadDistance > brakingDistance)
            return false;

        float timeToNearestCrossroad = TimeToPass(nearestCrossroadDistance);
        if (timeToNearestCrossroad > 30.0f)
            return true;

        if (roadFather.GetComponent<RelationsEditor>().frames[currentFrame].GetRoadToTrackById(nearestCrossroad.GetComponent<SimpleRoad>().id)
                .Count == 0)
            return false;

        int idRoad = nearestCrossroad.GetComponent<SimpleRoad>().id;
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
        Debug.Log(equidistantTime + equidimensionalTime);
        return equidistantTime + equidimensionalTime;
    }
}