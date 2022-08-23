using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCharacteristics : MonoBehaviour
{
    // Максимальная скорость
    public float maxSpeed;
    // Ускорение
    public float acceleration;
    // Габариты автомобиля (пока не важный параметр, поэтому и приват)
    private Vector3 size;
    // Скорость в моменте
    public float speed;
    // Дорога, которой принадлежит авто
    public RoadCharacteristics parentRoad;
    // Расстояние, которое прошла машина по дороге
    public float distance;
}
