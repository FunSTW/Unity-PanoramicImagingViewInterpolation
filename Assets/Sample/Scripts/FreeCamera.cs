using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    public float sensitivity = 10f;
    public float maxYAngle = 80f;
    private Vector2 currentRotation;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
            currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
            currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
            currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
            Camera.main.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
        }
    }
}