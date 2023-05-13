using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAndThrow : MonoBehaviour
{
    Rigidbody grabbed;

    Camera eyeCam;

    public float eyeRange = 50.0f;
    public float throwForce = 250.0f;

    void Start()
    {
        eyeCam = GetComponent<Camera>();
    }

    void Update()
    {
        //If the left mouse is pressed
        if( Input.GetKeyDown(KeyCode.Mouse0) )
        {
            //If we aren't grabbing an object, get the raycasted object
            if ( grabbed == null )
                grabbed = GetGrabbed();
            //Otherwise just nullify what we have grabbed
            else
                grabbed = null;
        }

        //While we have a grabbed object
        if( grabbed != null )
        {
            //Set the position to the camera position and forward vector by 25
            //Also set the grabbed objects velocity to zero so it doesn't think its falling
            grabbed.transform.position = eyeCam.transform.position + (eyeCam.transform.forward * 2.5f);
            grabbed.velocity = Vector3.zero;

            //If the right mouse is pressed
            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                //Throw the object with force at forward vector of camera and nullify grabbed
                grabbed.AddForce(eyeCam.transform.forward * throwForce);
                grabbed = null;
            }
        }
    }

    public Rigidbody GetGrabbed()
    {
        RaycastHit hit;

        if (Physics.Raycast(eyeCam.transform.position, eyeCam.transform.forward, out hit, eyeRange, -1, QueryTriggerInteraction.Collide))
        {
            if (hit.rigidbody != null && hit.rigidbody.tag.Contains("Usable"))
                return hit.rigidbody;
        }

        return null;
    }
}
