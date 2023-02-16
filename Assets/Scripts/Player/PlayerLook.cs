using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    //The mouse sensitivity
    public float mouseSens = 100.0f;

    public Transform playerBody;

    float xRot = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Grab mouse left/right and up/down times by sensitity and deltatime
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        //Move the x rotation and clamp the values
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90.0f, 90.0f);

        //Apply rotation
        transform.localRotation = Quaternion.Euler(xRot, 0.0f, 0.0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
