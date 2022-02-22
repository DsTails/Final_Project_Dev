using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float xRot = 0;

    public float sensitivity;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xMouse = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float yMouse = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRot -= yMouse;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRot, xMouse, 0f);
    }
}
