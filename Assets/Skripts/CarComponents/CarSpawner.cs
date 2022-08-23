using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    // Префаб создаваемой машины.
    public GameObject carPrefab;

    // Дорога, с которой начинает движение авто
    public RoadCharacteristics road;

    // Время, которое будет ждать спавнер
    public float time;

    //
    private float timeUpdate;


    // Start is called before the first frame update
    private void Awake()
    {
        timeUpdate = time;
    }

    private void FixedUpdate()
    {
        timeUpdate -= Time.deltaTime;
        if (timeUpdate <= 0)
        {
            GameObject createdCar = Instantiate(carPrefab, road._startPost.transform);
            createdCar.GetComponent<CarCharacteristics>().parentRoad = road;
            createdCar.AddComponent<CarMovement>();

            timeUpdate = time;
        }
    }
}
