using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FrameRoadsSelector;

public class CarBehaviourOnCrossroad : CarBehaviour
{
    // Пора ли тормозить?
    public override bool IsItTimeToSlowDown()
    {
        // Ближайшая машина
        float nearestCarDistance = GetNearesCarDistance();
        GameObject nearestCrossroad = new GameObject();
        float nearestCrossroadDistance = float.MaxValue;

        if (nearestCarDistance < nearestCrossroadDistance)
            return nearestCarDistance < brakingDistance + GlobalSettings.SaveDistance;

        if (TimeToPass(nearestCrossroadDistance) > 30.0f)
            return true;

        if (nearestCrossroad.GetComponent<CrossRoadEntrance>().childRoads.Count == 0)
            return false;

        int idRoad = nearestCrossroad.GetComponent<CrossRoadEntrance>().childRoads[0].GetComponent<SimpleRoad>().id;
        GameObject roadFather = GameObject.Find("RoadFather");
        foreach (var road in roadFather.GetComponent<FrameRoadsSelector>().frames[0].GetRoadToTrackById(idRoad))
        {
            
        }

        return false;

    }

    
    // 
    private float GetNearesCarDistance()
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
            float distanceToNearestCar = nearestCar.GetComponent<CarBehaviour>().distance - this.distance - thisCarDimensions - nearestCarDimensions;

            return distanceToNearestCar;
        }

        return float.MaxValue;
    }
    // Возвращает ближайшую машину на пути
    private GameObject FindNearestCar()
    {
        Transform crossroadEntrancePtr = crossroadEntrance;
        while (crossroadEntrancePtr != null &&
               crossroadEntrancePtr.GetComponent<CrossRoadEntrance>().childRoads.Count > 0) //
        {
            foreach (var car in crossroadEntrance.GetComponent<CrossRoadEntrance>().childRoads[0] //
                         .GetComponent<SimpleRoad>().carsOnThisRoad)
            {
                float distanceToCar = car.GetComponent<CarBehaviour>().distance - this.distance;
                if (car != gameObject && distanceToCar > 0.0f)
                    return car;
            }

            crossroadEntrancePtr = crossroadEntrance.GetComponent<CrossRoadEntrance>().childRoads[0] //
                .GetComponent<SimpleRoad>().transform.Find("CrossRoadEntrance");
        }

        return null;
    }
    
    /*
    private GameObject FindNearestCrossroad()
    {
        Transform crossroadEntrancePtr = crossroadEntrance;
        while (crossroadEntrancePtr != null &&
               crossroadEntrancePtr.GetComponent<CrossRoadEntrance>().childRoads.Count > 0) //
        { 
            foreach (var road in crossroadEntrance.GetComponent<CrossRoadEntrance>().childRoads)
                if (road )
        }
        return new GameObject();
    }
    */

    private float TimeToPass(float distance)
    {
        float equidistantTime = (maxSpeedPerTick - speedPerTick) / accelerationPerTick;
        float equidistantDistance = speedPerTick * equidistantTime + accelerationPerTick * equidistantTime * equidistantTime / 2;
        float equidimensionalTime = (distance - equidistantDistance) / maxSpeedPerTick;
        return equidistantTime + equidimensionalTime;
    }
}
