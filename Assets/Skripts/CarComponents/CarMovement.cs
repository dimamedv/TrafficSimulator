using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    // Положение в пространстве
    public Transform _transform;

    // Скорость в моменте
    public float speed;

    // Дорога, которой принадлежит авто
    public GameObject parentRoad;
}
