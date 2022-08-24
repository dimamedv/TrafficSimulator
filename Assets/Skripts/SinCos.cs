using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinCos : MonoBehaviour
{
    // Start is called before the first frame update

    private float alfa = 0;
    public GameObject gm;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        alfa += 0.01f;
        gm.transform.position = new Vector3((float)System.Math.Cos(alfa + System.Math.PI / 2) * 2, 0f, (float)System.Math.Sin(alfa + System.Math.PI / 2) * 2);


    }
}
