using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 forwardVector = transform.forward;
        transform.position = transform.position + forwardVector * 0.1f;
    }
}
