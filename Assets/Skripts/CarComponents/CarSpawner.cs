using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    // Префаб создаваемой машины.
    public GameObject carPrefab;
    // Дорога, с которой начинает движение авто
    public SimpleRoad road;
    // Время, которое будет ждать спавнер
    public float time;

    public List<GameObject> destinationPosts;

    // Отсчитывае
    private float _timeUpdate;


    private void Start()
    {
        _timeUpdate = time;
        FindDestinationPosts();
    }

    private void FixedUpdate()
    {
        _timeUpdate -= Time.deltaTime;
        if (_timeUpdate <= 0)
        {
            _timeUpdate = time;
            if (road.carsOnThisRoad.Count == 0 || road.carsOnThisRoad[^1].GetComponent<CarBehaviour>().distance > 5)
                CarSpawn();
        }
    }

    public void CarSpawn()
    {
        GameObject createdCar = Instantiate(carPrefab);
        createdCar.name = "Car";
        createdCar.transform.position = road.startPost.transform.position + carPrefab.transform.position;
        createdCar.transform.Rotate(-Vector3.up * 90);

        int random = Random.Range(0, destinationPosts.Count);

        CarBehaviour carBehaviour = createdCar.GetComponent<CarBehaviour>();
        carBehaviour.destinationPost = destinationPosts[random];
        carBehaviour.findPathToDestination(road.gameObject);
        carBehaviour.parentRoad = carBehaviour.getNextRoad().GetComponent<SimpleRoad>();
        carBehaviour.GetNearestCrossroad();

        road.carsOnThisRoad.Add(createdCar);
    }

    public void FindDestinationPosts()
    {
        destinationPosts = new List<GameObject>();
        foreach (var roadIn in SimpleRoad.RoadList)
        {
            SimpleRoad simple = roadIn.GetComponent<SimpleRoad>();
            if (simple.childConnection == null && road.GetComponent<SimpleRoad>().templateOwner != simple.templateOwner)
                destinationPosts.Add(simple.endPost);
        }
    }
}
