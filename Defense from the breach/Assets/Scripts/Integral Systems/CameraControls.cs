using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float sensitivity;
    float xRot = 0;

    [SerializeField] Transform playerChar;
    [SerializeField] Transform firepointOrigin;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float xMouse = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float yMouse = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRot -= yMouse;

        xRot = Mathf.Clamp(xRot, -90, 90);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        playerChar.Rotate(Vector3.up * xMouse);

        //firepointOrigin.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        //firepointOrigin.RotateAround(playerChar.position, new Vector3(xRot, 0f, 0f), 0);
    }
}
