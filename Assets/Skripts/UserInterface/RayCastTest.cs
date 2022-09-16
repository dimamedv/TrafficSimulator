using UnityEngine;
using System.Collections;

public class RayCastTest : MonoBehaviour {
    public Camera camera;

    void Update(){
        if (Input.GetMouseButtonDown(0))
            Debug.Log("Pressed primary button.");
        if (Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                Debug.Log(objectHit.name);
            }
        }
    }
}