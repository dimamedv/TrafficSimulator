using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarSpawnerBehaviour : MonoBehaviour
{
    //Список постов, на которые могут ехать появившиеся машины.
    public List<GameObject> destinationPosts;
    //Префаб создаваемой машины.
    public GameObject carPrefab;
    
    //Дорога, на которой появляется машина.
    private GameObject _parentRoad;
    //Пост, на котором появляется машина.
    private GameObject _parentPost;
    
    // Start is called before the first frame update
    void Start()
    {
        _parentRoad = transform.parent.parent.GameObject();
        _parentPost = transform.parent.GameObject();

        Vector3 spawnPosition = transform.position;
        spawnPosition.y += 1;
        GameObject createdCar = Instantiate(carPrefab, spawnPosition, transform.rotation);
        createdCar.GetComponent<CarBehaviour>().currentRoad = _parentRoad;

        _parentRoad.GetComponent<RoadBehaviour>().carsOnThisRoad.Add(createdCar);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
