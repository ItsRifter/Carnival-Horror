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
        if( Input.GetKeyDown(KeyCode.Mouse0) )
        {
            if ( grabbed == null )
                grabbed = GetGrabbed();
            else
                grabbed = null;
        }

        if( grabbed != null )
        {
            grabbed.transform.position = eyeCam.transform.position + (eyeCam.transform.forward * 2.5f);
            grabbed.velocity = Vector3.zero;

            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                grabbed.AddForce(eyeCam.transform.forward * throwForce);
                grabbed = null;
            }
        }
    }

    public Rigidbody GetGrabbed()
    {
        RaycastHit hit;

        if (Physics.Raycast(eyeCam.transform.position, eyeCam.transform.forward, out hit, eyeRange))
        {
            if (hit.rigidbody != null && hit.rigidbody.tag.Contains("Usable"))
                return hit.rigidbody;
        }

        return null;
    }
}
