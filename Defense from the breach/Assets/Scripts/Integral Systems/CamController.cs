using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public float xRot = 0;

    public float Sensitivity;
    public Transform playerBody;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xMouse = Input.GetAxis("Mouse X") * Sensitivity * GamePause.deltaTime;
        float yMouse = Input.GetAxis("Mouse Y") * Sensitivity * GamePause.deltaTime;

        xRot -= yMouse;
        xRot = Mathf.Clamp(xRot, -90, 90);

        transform.localRotation = Quaternion.Euler(xRot, 0, 0);

        playerBody.Rotate(Vector3.up * xMouse);
    }
}
