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
        destinationPosts = new List<GameObject>();
        foreach (var roadIn in SimpleRoad.RoadList)
        {
            GameObject endPost = roadIn.GetComponent<SimpleRoad>().endPost;
            if (endPost == null && endPost != road.GetComponent<SimpleRoad>().endPost)
                destinationPosts.Add(endPost);
        }
    }

    private void FixedUpdate()
    {
        _timeUpdate -= Time.deltaTime;
        if (_timeUpdate <= 0)
        {
            _timeUpdate = time;
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
        Debug.Log(random);

        CarBehaviour carBehaviour = createdCar.GetComponent<CarBehaviour>();
        carBehaviour.parentRoad = road;
        carBehaviour.destinationPost = destinationPosts[random];
        carBehaviour.findPathToDestination(carBehaviour.parentRoad.gameObject);

        road.carsOnThisRoad.Add(createdCar);
    }
}
