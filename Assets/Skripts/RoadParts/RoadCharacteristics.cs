using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCharacteristics : MonoBehaviour
{
    // Стартовая точка
    public GameObject _startPost;

    // Конечная точка
    public GameObject _endPost;

    // Ширина дороги
    public float width;

    // Количество фрагментов дороги (Детализация)
    public int details;

    // Точки, через которые проходит автомобиль
    public List<Vector3> points;

    // Длины сегментов дороги
    public List<float> lengthSegments;

    // Префиксные суммы длин сегментов дороги
    public List<float> prefixSumSegments;

    private void Awake()
    {
        points = new List<Vector3>(details);
    }
}
