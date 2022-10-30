using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviourOnCrossroad : MonoBehaviour
{
    // ���� �� ���������?
    public bool IsItTimeToSlowDown()
    {
        // ��������� ������
        if (CarAtDangerousDistance(FindNearestCar()))
            return true;

        return false;

    }

    // ���������� ��������� ������ �� ����
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
    // 
    private bool CarAtDangerousDistance(GameObject nearestCar)
    {
        if (nearestCar != null)
        {
            // ���������� �� ��������� ������
            float distanceToNearestCar = nearestCar.GetComponent<CarBehaviour>().distance - this.distance;
            // �������� ���� ������
            float thisCarDimensions =
                gameObject.transform.localScale.x * gameObject.GetComponent<BoxCollider>().size.x / 2;
            // �������� ��������� ������
            float nearestCarDimensions =
                nearestCar.transform.localScale.x * nearestCar.GetComponent<BoxCollider>().size.x / 2;
            // ���������� ��������� ���������
            float saveStoppingDistance =
                brakingDistance + GlobalSettings.SaveDistance + nearestCarDimensions + thisCarDimensions;
            // ���� �� ���������?
            bool isItTimeToSlowDown = saveStoppingDistance > distanceToNearestCar;

            if (isItTimeToSlowDown)
            {
                return true;
            }
        }

        return false;
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

}
