using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    // Префаб создаваемой машины.
    public GameObject carPrefab;
    // Дорога, с которой начинает движение авто
    public AbstractRoad road;
    // Время, которое будет ждать спавнер
    public float time;
    // Отсчитывае
    private float _timeUpdate;


    // Start is called before the first frame update
    private void Awake()
    {
        _timeUpdate = time;
    }

    private void FixedUpdate()
    {
        _timeUpdate -= Time.deltaTime;
        Debug.Log(_timeUpdate);
        if (_timeUpdate <= 0)
        {
            GameObject createdCar = Instantiate(carPrefab, road.startPost.transform);
            createdCar.GetComponent<CarBehaviour>().parentRoad = road;
            createdCar.transform.Rotate(-Vector3.up * 90);

            _timeUpdate = time;
        }
    }
}
