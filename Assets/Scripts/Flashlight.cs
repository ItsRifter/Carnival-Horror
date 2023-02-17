using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashLightComponent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If F is pressed, toggle flashlight
        if(Input.GetKeyDown(KeyCode.F))
        {
            flashLightComponent.enabled = !flashLightComponent.enabled;
        }
    }
}
