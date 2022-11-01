using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FrameRoadsSelector;
using static CrossRoadManager;

public class CarBehaviourOnCrossroad : CarBehaviour
{
    public GameObject roadFather;

    public float nearestCarDistance;

    // Габариты этой машины
    public float thisCarDimensions;
    public float timeToNearestCrossroad;
    public CrossRoadFrame activeCrossroadFrame;

    private void Start()
    {
        roadFather = GameObject.Find("RoadFather");
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
        
        if (GameObject.Find("RoadFather").GetComponent<CrossRoadManager>().timeBeforeFrameChange < 2)
            return true;

        timeToNearestCrossroad = TimeToPass(crossroadEnd);
        if (timeToNearestCrossroad > 30.0f)
            return true;

        activeCrossroadFrame = roadFather.GetComponent<FrameRoadsSelector>()
            .frames[roadFather.GetComponent<CrossRoadManager>().currentFrameIndex];
        List<int> primaryRoadForThis =
            activeCrossroadFrame.GetRoadToTrackById(nearestCrossroad.GetComponent<SimpleRoad>().id);
        if (primaryRoadForThis.Count == 0)
            return false;

        if (FindCarForGiveWay(primaryRoadForThis))
            return true;

        return false;
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
                float distanceToCar = curParentRoad.carsOnThisRoad[j].GetComponent<CarBehaviour>().distance -
                                      this.distance;
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

    private bool FindCarForGiveWay(List<int> primaryRoadForThis)
    {
        foreach (int id in primaryRoadForThis)
        {
            SimpleRoad road = activeCrossroadFrame.GetRoadById(id).GetComponent<SimpleRoad>();
            while (road != null)
            {
                List<GameObject> cars = road.carsOnThisRoad;
                if (cars.Count > 0)
                {
                    CarBehaviourOnCrossroad carPtr = cars[0].GetComponent<CarBehaviourOnCrossroad>();
                    int idCarPtrCrossroad = carPtr.nearestCrossroad.GetComponent<SimpleRoad>().id;
                    foreach (var index in primaryRoadForThis)
                        if (idCarPtrCrossroad == index && carPtr.timeToNearestCrossroad < this.timeToNearestCrossroad)
                            return true;
                }

                if (road.parentConnection != null &&
                    road.parentConnection.GetComponent<CrossRoadEntrance>().parentRoads[0] != null)
                    road = road.parentConnection.GetComponent<CrossRoadEntrance>().parentRoads[0]
                        .GetComponent<SimpleRoad>();
                else
                    road = null;
            }
        }

        return false;
    }
}