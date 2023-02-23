using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DistractiveItem : MonoBehaviour
{
    [SerializeField]
    LayerMask mask;

    [SerializeField]
    float minVelocity = -1.5f;

    [SerializeField]
    float distractRange = 64.0f;

    Rigidbody rb;
    bool canDistract;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //If the object is falling at the required velocity, set boolean to true
        if(rb.velocity.y < minVelocity && !canDistract)
            canDistract = true;

        //if we just hit the ground and the object can distract
        //find the AI and distract them
        //then set the boolean to false
        if(rb.velocity.y > 0.1f && canDistract)
        {
            foreach (var obj in Physics.OverlapSphere(transform.position, distractRange))
            {
                if(obj.tag == "Creature")
                {
                    obj.GetComponent<CreatureNav>().destination = transform.position;
                }
            }

            canDistract = false;
        }
    }
}
