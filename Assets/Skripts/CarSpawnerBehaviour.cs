using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarSpawnerBehaviour : MonoBehaviour
{
    public List<GameObject> destinationPosts;
    public GameObject car;

    private GameObject _parentRoad;
    private GameObject _parentPost;
    // Start is called before the first frame update
    void Start()
    {
        _parentRoad = transform.parent.parent.GameObject();
        _parentPost = transform.parent.GameObject();

        Vector3 spawnPosition = transform.position;
        spawnPosition.y += 1;
        GameObject createdCar = Instantiate(car, spawnPosition, transform.rotation);

        _parentRoad.GetComponent<RoadBehaviour>().carsOnThisRoad.Add(createdCar);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
