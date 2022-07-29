using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPostBehaviour : MonoBehaviour
{
    public bool hasSpawner;
    public GameObject carSpawner;
    private GameObject _spawner;
    
    // Start is called before the first frame update
    void Start()
    {
        if (hasSpawner)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y += 2;
            _spawner = Instantiate(carSpawner, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
