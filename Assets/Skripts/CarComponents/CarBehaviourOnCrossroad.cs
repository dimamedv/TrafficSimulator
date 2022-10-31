using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FrameRoadsSelector;

public class CarBehaviourOnCrossroad : CarBehaviour
{
    public int currentFrame = 0;
    GameObject roadFather;

    public float nearestCarDistance;

    // Габариты этой машины
    public float thisCarDimensions;

    private void Start()
    {
        thisCarDimensions = gameObject.transform.localScale.x * gameObject.GetComponent<BoxCollider>().size.x / 2;
    }

    // Пора ли тормозить?
    public override bool IsItTimeToSlowDown()
    {
        nearestCarDistance = GetNearestCarDistance();

        if (nearestCarDistance < nearestCrossroadDistance)
            return nearestCarDistance < brakingDistance + GlobalSettings.SaveDistance;

        if (nearestCrossroadDistance > brakingDistance + thisCarDimensions + GlobalSettings.SaveDistance)
            return false;

        float timeToNearestCrossroad = TimeToPass(crossroadEnd);
        if (timeToNearestCrossroad > 30.0f)
            return true;

        if (roadFather.GetComponent<RelationsEditor>().frames[currentFrame].GetRoadToTrackById(nearestCrossroad.GetComponent<SimpleRoad>().id)
                .Count == 0)
            return false;

        return true;
        /*
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
        */
    }


    // 
    private float GetNearestCarDistance()
    {
        GameObject nearestCar = FindNearestCar();
        if (nearestCar != null)
        {
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
        SimpleRoad curParentRoad = parentRoad;
        int i = path.Count;
        while (i > 0)
        {
            for (int j = curParentRoad.carsOnThisRoad.Count - 1; j >= 0; j--)
            {
                float distanceToCar = curParentRoad.carsOnThisRoad[j].GetComponent<CarBehaviour>().distance - this.distance;
                if (curParentRoad.carsOnThisRoad[j] != gameObject && distanceToCar > 0.0f)
                    return curParentRoad.carsOnThisRoad[j];
            }
            i--;
            curParentRoad = path[i].GetComponent<SimpleRoad>();
        }

        return null;
    }


    public float TimeToPass(float distance)
    {
        float equidistantTime = (maxSpeedPerTick - speedPerTick) / accelerationPerTick;
        float equidistantDistance =
            speedPerTick * equidistantTime + accelerationPerTick * equidistantTime * equidistantTime / 2;
        float equidimensionalTime = (crossroadEnd * Time.deltaTime - equidistantDistance) / maxSpeedPerTick;
        float res = equidistantTime + equidimensionalTime;
        return res;
    }
}