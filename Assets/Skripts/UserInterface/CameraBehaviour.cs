using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public float _runSpeed = 10;
    public float _scaleMultiplier = 1.2f;
    public Camera _camera;
    public Vector2 ScaleDistance;

    private Vector3 _previousPosition;
    private float cameraDistance;


    // Update is called once per frame
    private void LateUpdate()
    {
        MoveCamera();
        rotateCamera();
        ScaleCamera();
    }

    private void MoveCamera()
    {
        float x = 0;
        float z = 0;

        if (Input.GetKey("d"))
            x += _runSpeed * Time.deltaTime;
        if (Input.GetKey("a"))
            x -= _runSpeed * Time.deltaTime;
        if (Input.GetKey("w"))
            z += _runSpeed * Time.deltaTime;
        if (Input.GetKey("s"))
            z -= _runSpeed * Time.deltaTime;

        transform.parent.Translate(
            new Vector3(x, 0, z)
        ) ;
    }

    // Вращение камеры вокруг центра вращения (Родительского объекта)
    private void rotateCamera()
    {

        if (Input.GetMouseButtonDown(2))
        {
            _previousPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 direction = _previousPosition - _camera.ScreenToViewportPoint(Input.mousePosition); ;

            _camera.transform.position = transform.parent.transform.position;

            _camera.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
            //cam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
            transform.parent.Rotate(new Vector3(0, 1, 0), -direction.x * 180);
            _camera.transform.Translate(new Vector3(0, 0, -cameraDistance));

            _previousPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
        }
    }

    // Увеличение/Уменьшение масштаба камеры 
    void ScaleCamera()
    {
        float mw = Input.GetAxis("Mouse ScrollWheel");
        cameraDistance = (float)Math.Sqrt(
            Math.Pow(_camera.transform.localPosition.x, 2)
                + Math.Pow(_camera.transform.localPosition.y, 2)
                + Math.Pow(_camera.transform.localPosition.z, 2)
        );

        Vector3 CameraPosition = new Vector3(
                _camera.transform.localPosition.x,
                _camera.transform.localPosition.y,
                _camera.transform.localPosition.z
            );

        if (mw > 0f && cameraDistance > (float)ScaleDistance.x)
        {
            _camera.transform.localPosition = CameraPosition / _scaleMultiplier;
            _runSpeed /= _scaleMultiplier;
        }
        else if (mw < 0f && cameraDistance < (float)ScaleDistance.y)
        {
            _camera.transform.localPosition = CameraPosition * _scaleMultiplier;
            _runSpeed *= _scaleMultiplier;
        }
    }
}
