using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    // Префаб создаваемой машины.
    public GameObject carPrefab;
    // Время, которое будет ждать спавнер
    public float time;

    // Отсчитывае
    private float _timeUpdate;


    private void Awake()
    {
        _timeUpdate = time;
    }

    private void FixedUpdate()
    {
        _timeUpdate -= Time.deltaTime;
        if (_timeUpdate <= 0)
        {
            CarSpawn();
            _timeUpdate = time;
        }
    }

    public void CarSpawn()
    {
        GameObject createdCar = Instantiate(carPrefab);
        createdCar.name = "Car";
        createdCar.GetComponent<CarBehaviour>().parentRoad = gameObject.GetComponent<SimpleRoad>();
        createdCar.transform.position = gameObject.GetComponent<SimpleRoad>().startPost.transform.position + carPrefab.transform.position;
        createdCar.transform.Rotate(-Vector3.up * 90);
    }
}
